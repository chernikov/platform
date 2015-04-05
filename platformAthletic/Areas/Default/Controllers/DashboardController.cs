﻿using platformAthletic.Attributes;
using platformAthletic.Global;
using platformAthletic.Model;
using platformAthletic.Models.Info;
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
    public class DashboardController : DefaultController
    {
        private int pageSize = 20;
        [SeasonCheck]
        public ActionResult Index(string searchString = null, int? groupId = null, int page = 1)
        {
            var team = CurrentUser.OwnTeam;
            ViewBag.SearchString = searchString;
            ViewBag.GroupId = groupId;
            if (CurrentUser.OwnTeam.SubGroups.Any())
            {
                ViewBag.SelectedListGroups = GetSelectedListGroups(CurrentUser.OwnTeam.SubGroups, groupId);
            }

            if (team != null)
            {
                var list = team.ActiveUsers.OrderBy(p => p.LastName).ToList();
                if (groupId.HasValue)
                {
                    list = list.Where(p => p.GroupID == groupId.Value).ToList();
                }
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    list = SearchEngine.SearchTeamUser(searchString, list.AsQueryable()).ToList();
                }
                var data = new PageableData<User>();
                data.Init(list.AsQueryable(), page, "Index", itemPerPage : pageSize);
                return View(data);
            }
            return RedirectToLoginPage;
        }

        public ActionResult JsonPlayers()
        {
            var team = CurrentUser.OwnTeam;

            return Json(new
            {
                team = team.ActiveUsers.ToList().Select(p => new
                {
                    id = p.ID,
                    name = p.FirstName + " " + p.LastName,
                    state = p.Team.State.Name,
                    avatar = p.FullAvatarPath
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserInfo(int id)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                return View(user);
            }
            return null;
        }

        public ActionResult AttendanceCalendar(int id, DateTime date)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null) 
            {
                var attendanceInfo = new AttendanceInfo() {
                    UserID = user.ID,
                    Date = date,
                };
                var firstDate = new DateTime(date.Year, date.Month, 1);
                var lastDate = firstDate.AddMonths(1);
                attendanceInfo.Attendances = user.UserAttendances.Where(p => p.AddedDate >= firstDate && p.AddedDate < lastDate).Select(p => p.AddedDate.Date).ToList();
                return View(attendanceInfo);
            }
            return null;
        }

        private IEnumerable<SelectListItem> GetSelectedListGroups(IEnumerable<Group> groups, int? groupId)
        {
            yield return new SelectListItem()
            {
                Value = "",
                Text = "ALL GROUPS",
                Selected = groupId == null,
            };
            foreach (var group in groups)
            {
                yield return new SelectListItem()
                {
                    Value = group.ID.ToString(),
                    Text = group.Name.ToUpper(),
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
