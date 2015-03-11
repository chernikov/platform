using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class HomeController : AdminController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminMenu()
        {
            return View();
        }
    }
}
