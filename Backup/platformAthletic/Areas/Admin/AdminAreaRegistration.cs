using System.Web.Mvc;

namespace platformAthletic.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                null,
                "admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { "platformAthletic.Areas.Admin.Controllers" }
            );
        }
    }
}
