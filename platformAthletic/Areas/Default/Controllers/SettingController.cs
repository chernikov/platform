using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class SettingController : DefaultController
    {
        public ActionResult CurrentDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetCurrentDate(DateTime dateTime)
        {
            var setting = new Setting()
            {
                Name = "CurrentDate",
                Value = dateTime.Date.ToString()
            };
            Repository.SaveSetting(setting);

            return Json(new
            {
                result = "ok"
            });
        }
    }
}
