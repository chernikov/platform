using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Global;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools;
using platformAthletic.Tools.Mail;
using System.Net.Mail;
using platformAthletic.Attributes;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles="coach")]
    public class TeamController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index(string searchString = null, int? groupId = null)
        {
            var team = CurrentUser.OwnTeam;

            if (CurrentUser.OwnTeam.SubGroups.Any())
            {
                ViewBag.SelectedListGroups = GetSelectedListGroups(CurrentUser.OwnTeam.SubGroups, groupId);
            }

            if (team != null)
            {
                var list = team.Players.OrderBy(p => p.LastName).ToList();
                if (groupId.HasValue)
                {
                    list = list.Where(p => p.GroupID == groupId.Value).ToList();
                }
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    list = SearchEngine.SearchTeamUser(searchString, list.AsQueryable()).ToList();
                }

                return View(list);
            }
            return RedirectToLoginPage;
        }

        private IEnumerable<SelectListItem> GetSelectedListGroups(IEnumerable<Group> groups, int? groupId)
        {
            yield return new SelectListItem()
            {
                Value = "",
                Text = "Show All Groups",
                Selected = groupId == null,
            };
            foreach (var group in groups)
            {
                yield return new SelectListItem()
                {
                    Value = group.ID.ToString(),
                    Text = group.Name,
                    Selected = groupId == group.ID,
                };
            }

        }


        [HttpGet]
        public ActionResult AddPlayer()
        {
            var playerUserView = new PlayerUserView()
            {
                PlayerOfTeamID = CurrentUser.OwnTeam.ID
            };
            return View(playerUserView);
        }

        [HttpGet]
        public ActionResult MaxPlayers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPlayer(PlayerUserView playerUserView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(playerUserView, typeof(PlayerUserView), typeof(User));

                user.Password = StringExtension.CreateRandomPassword(8, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
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

                return View("_OK");

            }
            return View(playerUserView);
        }

        public ActionResult DeletePlayer(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID)
            {
                Repository.RemoveUser(user.ID);
                return Json(new { result = "ok", data = id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetSbc(int id, double value, SBCValue.SbcType type)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID) 
            {
                Repository.SetSbcValue(id, type, value);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult SetAttendance(int id, bool attendance)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID)
            {
                Repository.SetAttendance(id, attendance, user.CurrentSeason.ID);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }
    }
}
