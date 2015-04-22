using platformAthletic.Attributes;
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
        public ActionResult Index(string searchString = null, int? groupId = null, int page = 1, DateTime? selectedDate = null)
        {
            var team = CurrentUser.OwnTeam;
            ViewBag.SearchString = searchString;
            ViewBag.GroupId = groupId;
            ViewBag.SelectedDate = selectedDate ?? DateTime.Now;
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
            ViewBag.SelectedDate = selectedDate ?? DateTime.Now;
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
                    state = p.Team.State.Name,
                    avatar = p.FullAvatarPath
                })
            }, JsonRequestBehavior.AllowGet);
        }

       /* public ActionResult Last12WeekPerformance(int id)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var sunday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                var currentSunday = sunday.AddDays(-7 * 12);
                var labels = new List<string>();
                var sData = new List<int>();
                var bData = new List<int>();
                var cData = new List<int>();
                var tData = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    var sbc = user.SBCHistory(currentSunday);
                    if (sbc != null)
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                        tData.Add((int)(sbc.Squat + sbc.Bench + sbc.Clean));
                        currentSunday = currentSunday.AddDays(7);
                    }
                };
                var datasets = new List<PerformanceGraphInfo>();
                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Squat",
                    fillColor = "transparent",
                    strokeColor = "#ed4848",
                    pointColor = "#ed4848",
                    pointStrokeColor = "#ed4848",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = sData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Bench",
                    fillColor = "transparent",
                    strokeColor = "#3bcb67",
                    pointColor = "#3bcb67",
                    pointStrokeColor = "#3bcb67",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = bData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Clean",
                    fillColor = "transparent",
                    strokeColor = "#60b1c2",
                    pointColor = "#60b1c2",
                    pointStrokeColor = "#60b1c2",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = cData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Total",
                    fillColor = "transparent",
                    strokeColor = "#495b6c",
                    pointColor = "#495b6c",
                    pointStrokeColor = "#495b6c",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = tData
                });
                var data = new
                {
                    labels,
                    datasets
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }*/

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
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID)
            {
                Repository.RemoveUser(user.ID);
                return Json(new { result = "ok", data = id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeSbc(int id, SBCValue.SbcType type, double value)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID)
            {
                Repository.ChangeSbcValue(id, type, value);
                var newUser = Repository.Users.FirstOrDefault(p => p.ID == id);
                var newValue = 0;
                switch (type)
                {
                    case SBCValue.SbcType.Squat:
                        newValue = (int)newUser.Squat;
                        break;
                    case SBCValue.SbcType.Bench:
                        newValue = (int)newUser.Bench;
                        break;
                    case SBCValue.SbcType.Clean:
                        newValue = (int)newUser.Clean;
                        break;
                }
                return Json(new
                {
                    result = "ok",
                    value = newValue
                }, JsonRequestBehavior.AllowGet);
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
            if (user != null && user.PlayerOfTeamID == CurrentUser.OwnTeam.ID)
            {
                Repository.SetAttendance(id, attendance, user.CurrentSeason.ID, date);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

    }
}
