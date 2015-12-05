using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class TableController : AdminController
    {
        public ActionResult Index()
        {
            var info = new ScheduleTableInfo(Repository, new DateTime(2015, 6, 7), 100, CurrentUser.ID);
            return View(info);
        }

    }
}
