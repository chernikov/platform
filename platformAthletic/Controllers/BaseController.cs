using System.Web.Mvc;
using Ninject;
using Ninject.Web.Mvc;
using platformAthletic.Global.Auth;
using platformAthletic.Global.Config;
using platformAthletic.Model;
using System.Web.Routing;
using platformAthletic.Mappers;
using System.IO;
using platformAthletic.Global;
using System.Collections.Generic;
using platformAthletic.Tools;

namespace platformAthletic.Controllers
{
    public abstract class BaseController : Controller, IModelMapperController
    {
        public string HostName = string.Empty;

        protected static string NotFoundPage = "~/not-found-page";

        protected static string LoginPage = "~/Login";

        [Inject]
        public IRepository Repository { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IConfig Config { get; set; }

        [Inject]
        public IMapper ModelMapper { get; set; }

        [Inject]
        public PagePartProvider PageParts { get; set; }

        public User CurrentUser
        {
            get
            {
                return ((UserIndentity)Auth.CurrentUser.Identity).User;
            }
        }

        public RedirectResult RedirectToNotFoundPage
        {
            get
            {
                return Redirect(NotFoundPage);
            }
        }

        public RedirectResult RedirectToLoginPage
        {
            get
            {
                return Redirect(LoginPage);
            }
        }

        public RedirectResult RedirectBack(string defaultUrl)
        {
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }
            return Redirect(defaultUrl);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                HostName = requestContext.HttpContext.Request.Url.Authority;
            }

            base.Initialize(requestContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            if (!Config.DebugMode) 
            {
                filterContext.Result = View(new RouteValueDictionary (new { area = "Default", controller = "Error", action = "Index" }));
            }
        }

        [Inject]
        public PageCollection Pages { get; set; }

        [Inject]
        public BannerCollection Banners { get; set; }

        [Inject]
        public AphorismCollection Aphorisms { get; set; }

        protected Stream GetInputStream(string qqfile, out string fileName)
        {
            fileName = string.Empty;
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var httpPostedFileBase = Request.Files["qqfile"];
                if (httpPostedFileBase != null)
                {
                    fileName = qqfile;
                    return httpPostedFileBase.InputStream;
                }
            }
            else
            {
                if (qqfile != null)
                {
                    fileName = qqfile;
                    return Request.InputStream;
                }
            }
            return null;
        }

        protected bool TakeMoney(BillingInfo billingInfo, double amount, PaymentDetail paymentDetail)
        {

            var result = AuthorizeProcessor.GetRequest(Config.Authorize,
                billingInfo.CreditCardNumber,
                billingInfo.TextExpDate,
                billingInfo.CVC,
                amount,
                paymentDetail.Description,
                billingInfo.FirstName,
                billingInfo.LastName,
                billingInfo.BillingAddress,
                billingInfo.State.Code,
                billingInfo.ZipCode);

            Repository.SetResultPaymentDetail(paymentDetail.ID, result);
            Repository.ProcessPaymentDetail(paymentDetail);

            if (result.StartsWith("1|"))
            {
                return true;
            }
            return false;
        }
    }
}
