using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools;
using platformAthletic.Tools.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach")]
    public class ManagePlayerController : DefaultController
    {
        public ActionResult Index()
        {
            var list = CurrentUser.OwnTeam.Players.Where(p => !p.IsDeleted).OrderBy(p => p.LastName).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit", new PlayerUserView());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditTeamData(CurrentUser))
            {
                var playerUserView = (PlayerUserView)ModelMapper.Map(user, typeof(User), typeof(PlayerUserView));
                return View(playerUserView);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Edit(PlayerUserView playerUserView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(playerUserView, typeof(PlayerUserView), typeof(User));
                if (user.ID == 0)
                {
                    user.Password = StringExtension.CreateRandomPassword(8, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                    user.PlayerOfTeamID = CurrentUser.OwnTeam.ID;
                    Repository.CreateUser(user);
                    var userRole = new UserRole()
                    {
                        UserID = user.ID,
                        RoleID = 3 //player
                    };
                    Repository.CreateUserRole(userRole);
                    NotifyMail.SendNotify(Config, "RegisterPlayer", user.Email,
                                 (u, format) => string.Format(format, HostName),
                                 (u, format) => string.Format(format, u.Email, u.Password, HostName),
                                 user);

                    var existFailMail = Repository.FailedMails.FirstOrDefault(p => string.Compare(p.FailEmail, user.Email, true) == 0);
                    if (existFailMail != null)
                    {
                        Repository.RemoveFailedMail(existFailMail.ID);
                    }
                }
                else
                {
                    Repository.UpdateManageUser(user);
                }
                return View("_OK");

            }
            return View(playerUserView);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditTeamData(CurrentUser))
            {
                var playerUserView = (PlayerUserView)ModelMapper.Map(user, typeof(User), typeof(PlayerUserView));
                return View(playerUserView);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Delete(PlayerUserView playerUserView)
        {
            var id = playerUserView.ID;
            Repository.RemoveUser(id);
            return View("_OK");
        }

        [HttpGet]
        public ActionResult SendActivation(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditTeamData(CurrentUser))
            {
                NotifyMail.SendNotify(Config, "RegisterPlayer", user.Email,
                            (u, format) => string.Format(format, HostName),
                            (u, format) => string.Format(format, u.Email, u.Password, HostName),
                            user);
                Repository.ResendRegister(user);
                return View(user);
            }
            return View("_OK");
        }
    }
}
