using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class PageController : DefaultController
    {
        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermAndCondition()
        {
            return View();
        }
    }
}
