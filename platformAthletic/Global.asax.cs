using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Mvc;
using StackExchange.Profiling;
using platformAthletic.Areas.Admin;
using platformAthletic.Areas.Default;
using platformAthletic.Global.Auth;
using platformAthletic.Global.Config;
using platformAthletic.Mappers;
using platformAthletic.Model;
using platformAthletic.Tools.Mail;
using platformAthletic.Global;
using Ninject.Web.Common;
using platformAthletic.Areas.Cms;
using System.Collections.Specialized;
using platformAthletic.Areas.Default.Controllers;
using platformAthletic.Models.Info;
using System.Collections.Generic;
using System.IO;
using platformAthletic.Controllers;


namespace platformAthletic
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static IKernel AppKernel { get; private set; }

        private Thread mailThread { get; set; }

        protected override IKernel CreateKernel()
        {
            var res = new StandardKernel();
            res.Bind<IConfig>().To<Config>();
            res.Bind<IMapper>().To<CommonMapper>();
            res.Bind<platformAthleticDbDataContext>().ToMethod(c => new platformAthleticDbDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
            res.Bind<IRepository>().To<SqlRepository>().InRequestScope(); 
            res.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
            MailSender.config = res.Get<IConfig>();
            MailReceiver.config = res.Get<IConfig>();
            AppKernel = res;
            
            return res;
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("glimpse.axd");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("elmah.axd/{*pathInfo}");
            routes.IgnoreRoute("media/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });
            routes.IgnoreRoute("{*robots}", new { robots = @"(.*/)?robots.txt(/.*)?" });
            routes.IgnoreRoute("*.asmx");
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Context.Session != null)
            {
                //Аутентификация
                var auth = AppKernel.Get<IAuthentication>();
                auth.AuthCookieProvider = new HttpContextCookieProvider(Context);
                HttpContext.Current.User = auth.CurrentUser;


                //Культура управляется из Web.config
                var ci = AppKernel.Get<IConfig>().Culture;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                SqlSingleton.sqlRepository = AppKernel.Get<SqlRepository>();
            }
        }

        protected override void OnApplicationStarted()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var adminArea = new AdminAreaRegistration();
            var adminAreaContext = new AreaRegistrationContext(adminArea.AreaName, RouteTable.Routes);
            adminArea.RegisterArea(adminAreaContext);

            var cmsArea = new CmsAreaRegistration();
            var cmsAreaContext = new AreaRegistrationContext(cmsArea.AreaName, RouteTable.Routes);
            cmsArea.RegisterArea(cmsAreaContext);


            var defaultArea = new DefaultAreaRegistration();
            var defaultAreaContext = new AreaRegistrationContext(defaultArea.AreaName, RouteTable.Routes);
            defaultArea.RegisterArea(defaultAreaContext);

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();

            mailThread = new Thread(ThreadFunc);
            mailThread.Start();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.IsLocal) //or any number of other checks, up to you 
            { 
            //    MiniProfiler.Start(); 
            } 
            var ajaxRequestHeader = Request.Headers.Get("X-Requested-With");
            var enableHttps = bool.Parse(ConfigurationManager.AppSettings["EnableHttps"]);
            if ((ajaxRequestHeader == null || ajaxRequestHeader != "XMLHttpRequest") && !Request.IsLocal && enableHttps && !Request.Url.AbsolutePath.Contains("/Media"))
            {
                var builder = new UriBuilder(Request.Url);

                if (Request.Url.Scheme == "http" && (Request.Url.AbsolutePath.Contains("/team-registration") || Request.Url.AbsolutePath.Contains("/individual-registration") || Request.Url.AbsolutePath.Contains("/billing")))
                {
                    Response.StatusCode = 301;
                    builder.Scheme = "https";
                    builder.Port = 443;
                    Response.AddHeader("Location", builder.ToString());
                    Response.End();

                }
                else if (Request.Url.Scheme == "https" && (!Request.Url.AbsolutePath.Contains("/team-registration") && !Request.Url.AbsolutePath.Contains("/individual-registration") && !Request.Url.AbsolutePath.Contains("/billing")))
                {
                    Response.StatusCode = 301;
                    builder.Scheme = "http";
                    builder.Port = 80;
                    Response.AddHeader("Location", builder.ToString());
                    Response.End();
                }
            }
        }


        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_BeginRequest()
        {
           
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop(); //stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
        }

        private static void ThreadFunc()
        {
            while (true)
            {
                try
                {
                    logger.Info("Start mail thread");
                    var workThread = new Thread(PeriodCheck);
                    workThread.Start();
                    logger.Info("Wait for end mail thread");
                    workThread.Join();
                    logger.Info("Sleep 60 seconds");
                }
                catch (Exception ex)
                {
                    logger.ErrorException("Thread period error", ex);
                }
                Thread.Sleep(60000);
            }
        }

        private static void PeriodCheck()
        {
            var repository = AppKernel.Get<IRepository>();
            var config = AppKernel.Get<IConfig>();
            FailMessage.Check(repository);
            FailMessage.Process(repository, config);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            if ((Request.Path ?? "").ToLower().StartsWith("/media/")
                || (Request.Path ?? "").ToLower().StartsWith("/content/")
                )
                return;

            string debug = System.Configuration.ConfigurationManager.AppSettings["DebugErrors"];
            if (debug == "1") return;

            var exception = Server.GetLastError();
            if (exception.Message.Contains("dangerous Request"))
            {
                return;
            }
               

            HttpException httpException = null;
            if (exception is HttpException) httpException = exception as HttpException;

            /*Response.Clear();
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;*/

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                if (httpException.GetHttpCode() == 404) routeData.Values["action"] = "NotFoundPage";
                else if (httpException.GetHttpCode() == 410) routeData.Values["action"] = "NotFoundPage";
                else if (httpException.GetHttpCode() == 403) routeData.Values["action"] = "Index";
                else
                {
                    if (httpException.GetType().Name == "HttpRequestValidationException")
                    {
                        Response.StatusCode = 403;
                        routeData.Values["action"] = "Index";

                    }
                    else routeData.Values["action"] = "Index";
                }
            }
            else
            {
                routeData.Values["action"] = "ServerError";
                Response.StatusCode = 500;
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    if (exception.Message.Contains("timeout") || exception.Message.Contains("Timeout"))
                    {
                        routeData.Values["action"] = "Index";
                        Response.StatusCode = 504;
                    }
                }
            }
            if (!Request.IsLocal && routeData.Values["action"].ToString() == "ServerError")
            {
                try
                {
                    string paramInfo = "";
                    NameValueCollection pColl = Request.Params;
                    for (int i = 0; i <= pColl.Count - 1; i++)
                    {
                        try
                        {
                            paramInfo += "[" + pColl.GetKey(i) + "] = ";
                            string[] pValues = pColl.GetValues(i);
                            for (int j = 0; j <= pValues.Length - 1; j++)
                            {
                                paramInfo += pValues[j] + ", ";
                            }
                            paramInfo += "\n";
                        }
                        catch { }
                    }
                    string formInfo = "";
                    NameValueCollection fColl = Request.Form;
                    for (int i = 0; i <= fColl.Count - 1; i++)
                    {
                        try
                        {
                            formInfo += "[" + fColl.GetKey(i) + "] = ";
                            string[] pValues = fColl.GetValues(i);
                            for (int j = 0; j <= pValues.Length - 1; j++)
                            {
                                formInfo += pValues[j] + ", ";
                            }
                            formInfo += "\n";
                        }
                        catch { }
                    }                       

                    /*MAIL*/
                    
                    string _serviceEmail = System.Configuration.ConfigurationManager.AppSettings["ServiceEmail"];
                    var serviceEmails = _serviceEmail.Replace(" ", "").Split(',');

                    foreach (var email in serviceEmails)
                    {
                        var mailController = new MailController();
                        var mailInfo = new Dictionary<string, object> 
                        {
                            { "Subject", "Server Error Athletic Platforms" },
                            { "Email",  email},
                            { "Link", Request.Path },
                            { "LinkFull", Request.Url.AbsolutePath },
                            { "Method", Request.HttpMethod },                      
                            { "Params", formInfo },                      
                            { "AllParams", paramInfo },                      
                            { "Error", exception },                      
                            { "ErrorCode", 500 }                      
                        };

                        var mail = mailController.ServerError(mailInfo);
                        using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
                        {
                            mailInfo["Body"] = reader.ReadToEnd();
                        }

                        MailSender.SendMail(mailInfo["Email"].ToString(), mailInfo["Subject"].ToString(), mailInfo["Body"].ToString());
                        
                    }

                    /* -- */
                }
                catch { }
            }



            //Response.Redirect("/" + routeData.Values["controller"] + "/" + routeData.Values["action"]);

            /*routeData.Values.Add("area", "default");
            IController errorsController = new platformAthletic.Areas.Default.Controllers.ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);             
            errorsController.Execute(rc);
             * */
        }
    }
}