using platformAthletic.Attributes;
using platformAthletic.Global;
using platformAthletic.Model;
using platformAthletic.Helpers;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools;
using platformAthletic.Tools.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;


namespace platformAthletic.Areas.Default.Controllers
{
    public class DashboardController : DefaultController
    {
        private int pageSize = 20;

        [SeasonCheck]
        public ActionResult Index(string searchString = null, int? groupId = null, int page = 1, DateTime? selectedDate = null)
        {
            var team = CurrentUser.OwnTeam;
            ViewBag.SearchString = searchString;
            ViewBag.GroupId = groupId;
            ViewBag.SelectedDate = selectedDate ?? DateTime.Now.Current();
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
                data.Init(list.AsQueryable(), page, "Index", itemPerPage: pageSize);

                if (list.Count == 0)
                {
                    return RedirectToAction("Extended");
                }
                return View(data);
            }
            return RedirectToLoginPage;
        }

        [SeasonCheck]
        public ActionResult Extended(string searchString = null, int? groupId = null, int page = 1, DateTime? selectedDate = null)
        {
            var team = CurrentUser.OwnTeam;
            ViewBag.SearchString = searchString;
            ViewBag.GroupId = groupId;
            ViewBag.SelectedDate = selectedDate ?? DateTime.Now.Current();
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
                data.Init(list.AsQueryable(), page, "Extended", itemPerPage: pageSize);
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
                    state = p.TeamOfPlay.State.Name,
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
                var attendanceInfo = new AttendanceInfo()
                {
                    User = user,
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
        public ActionResult AddPlayers()
        {
            var batchPlayersView = new BatchPlayersView();
            batchPlayersView.Init();
            return View(batchPlayersView);
        }

        [HttpGet]
        public ActionResult AddPlayerItem()
        {
            return View(new KeyValuePair<string, PlayerView>(Guid.NewGuid().ToString("N"), new PlayerView()));
        }

        [HttpGet]
        public ActionResult MaxPlayers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPlayers(BatchPlayersView batchPlayersView)
        {
            if (ModelState.IsValid)
            {
                foreach (var playerView in batchPlayersView.Players.Values)
                {
                    var user = (User)ModelMapper.Map(playerView, typeof(PlayerView), typeof(User));
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
                return View("_OK");
            }
            return View("AddPlayersBody", batchPlayersView);
        }

        public ActionResult DeletePlayer(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanBeDeleted(CurrentUser))
            {
                Repository.RemoveUser(user.ID);
                return Json(new { result = "ok", data = id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult Calendar(DateTime date)
        {
            return View(date);
        }

        public ActionResult CalendarBody(DateTime date)
        {
            var attendanceDateInfo = new AttendanceDateInfo()
            {
                Date = date
            };
            return View(attendanceDateInfo);
        }

        public ActionResult SetAttendance(int id, DateTime date, bool attendance)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditAttendance(CurrentUser))
            {
                Repository.SetAttendance(id, attendance, user.CurrentSeason.ID, date);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

    }
}
