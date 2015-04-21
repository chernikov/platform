using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class CoachController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.Users.Where(p => p.UserRoles.Any(r => r.RoleID == 2));
            return View(list);
        }

    }
}
