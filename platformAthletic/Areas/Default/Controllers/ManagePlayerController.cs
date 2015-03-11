using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
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
            var list = CurrentUser.OwnTeam.Users.OrderBy(p => p.LastName).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult EditPlayer(int id)
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
        public ActionResult EditPlayer(PlayerUserView playerUserView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(playerUserView, typeof(PlayerUserView), typeof(User));

                Repository.UpdateManageUser(user);
                return View("_OK");

            }
            return View(playerUserView);
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
