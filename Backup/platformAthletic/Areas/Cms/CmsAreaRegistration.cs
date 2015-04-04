using System.Web.Mvc;

namespace platformAthletic.Areas.Cms
{
    public class CmsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "cms",
                new { controller = "Home", action = "Index" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/who-we-are",
                new { controller = "Home", action = "WhoWeAre" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/what-we-do",
                new { controller = "Home", action = "WhatWeDo" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/contact-us",
                new { controller = "Home", action = "ContactUs" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/join-us",
                new { controller = "Home", action = "JoinUs" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/team-register",
                new { controller = "Home", action = "TeamRegister" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
                null,
                "cms/individual-register",
                new { controller = "Home", action = "IndividualRegister" },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
               null,
               "cms/post-register",
               new { controller = "Home", action = "PostRegister" },
               new[] { "platformAthletic.Areas.Cms.Controllers" }
            );

            context.MapRoute(
              null,
              "cms/save",
              new { controller = "Home", action = "Save" },
              new[] { "platformAthletic.Areas.Cms.Controllers" }
           );


            context.MapRoute(
                null,
                "cms/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { "platformAthletic.Areas.Cms.Controllers" }
            );
        }
    }
}