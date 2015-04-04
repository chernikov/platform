using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class AboutUsController : DefaultController
    {
        public ActionResult Index()
        {
            var list = Repository.Abouts.ToList();
            return View(list);
        }

    }
}
