using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class ErrorController : DefaultController
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View("Index");
        }

        public new ActionResult NotFoundPage()
        {
            /*Response.StatusCode = 404;*/
            return View();
        }

        public void TestError()
        {
            var t = 0;
            var y = 45/t;
        }

    }
}
