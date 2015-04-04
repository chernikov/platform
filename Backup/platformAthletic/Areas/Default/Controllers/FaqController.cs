using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Models.Info;

namespace platformAthletic.Areas.Default.Controllers
{
    public class faqController : DefaultController
    {
        public ActionResult Index()
        {
            var list = Repository.Faqs.OrderBy(p => p.OrderBy).ToList();
            return View(list);
        }
    }
}
