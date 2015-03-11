using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Models.Info;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "payed")]
    public class GettingStartedController : DefaultController
    {
        public ActionResult Index()
        {
            if (CurrentUser != null)
            {
                Repository.VisitGettingStarted(CurrentUser.ID);
            }
            return View();
        }
    }
}
