using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Global;
using platformAthletic.Models.Info;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        public ActionResult Index(int page = 1, string search = null)
        {
            var list = Repository.Users.Where(p => !p.IsPhantom);
            if (search != null)
            {
                list = SearchEngine.Search(search, list).AsQueryable();
            }

            var data = new PageableData<User>();
            data.Init(list, page, "Index");
            ViewData["search"] = search;
            return View(data);
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {

            var adminUserView = new AdminUserView();
            return View("EditAdmin", adminUserView);

        }

        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (user != null)
            {
                var adminUserView = (AdminUserView)ModelMapper.Map(user, typeof(User), typeof(AdminUserView));
                return View(adminUserView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult EditAdmin(AdminUserView adminUserView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(adminUserView, typeof(AdminUserView), typeof(User));
                if (user.ID == 0)
                {
                    Repository.CreateAdminUser(user);
                    var userRole = new UserRole()
                    {
                        RoleID = 1, //admin
                        UserID = user.ID
                    };
                    Repository.CreateUserRole(userRole);
                }
                else
                {
                    Repository.UpdateAdminUser(user);
                }
                return RedirectToAction("Index");
            }
            return View(adminUserView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            if (user != null)
            {
                var userView = (UserView)ModelMapper.Map(user, typeof(User), typeof(UserView));
                return View(userView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(UserView userView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(userView, typeof(UserView), typeof(User));
                Repository.UpdateFullUser(user);
                return RedirectToAction("Index");
            }
            return View(userView);
        }

        [HttpGet]
        public ActionResult Remove(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                if (user.InRoles("coach"))
                {
                    Repository.RemoveWholeTeamByCoachId(user.ID);
                }
                Repository.RemoveUser(user.ID);

            }
            return RedirectToAction("Index");
        }

        public ActionResult Login(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Auth.Login(user.Email);
            }
            return RedirectToAction("Index", "Home", new { area = "Default" });
        }

        public ActionResult StartTutorial(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.StartTutorial(id);
            }
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Excel()
        {
            var list = Repository.Users.ToList().Select(p =>
                    new UserExportInfo() 
                    {
                        Email = p.Email ?? "",
                        Role = p.Role ?? "",
                        Name = p.FirstName + "  " + p.LastName,
                        Team = (p.TeamOfPlay ?? p.OwnTeam) != null ? (p.TeamOfPlay ?? p.OwnTeam).Name : "",
                        PhoneNumber = p.PhoneNumber ?? "",
                        Codes = string.Join(", ", p.PromoCodes.ToList()) ?? "",
                        PaidTill = p.PaidTill.HasValue ? p.PaidTill.Value.ToString("d") : ""
                    }).AsQueryable();
            var headers = new Dictionary<string, string>();
            headers.Add("Email", "User ID");
            headers.Add("Role", "Type of user");
            headers.Add("Name", "First and Last name");
            headers.Add("Team", "Team name");
            headers.Add("PhoneNumber", "Phone number");
            headers.Add("Codes", "Referral code used");
            headers.Add("PaidTill", "Subscription paid to date");

            return this.Excel(list, "users.xls", headers);
        }
    }
}