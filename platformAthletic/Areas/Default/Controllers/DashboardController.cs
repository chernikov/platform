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
using System.IO;
using RegularExpressions = System.Text.RegularExpressions;
using Regex = System.Text.RegularExpressions.Regex;
using System.ComponentModel.DataAnnotations;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles="coach,assistant")]
    public class DashboardController : DefaultController
    {
        private int pageSize = 20;

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
                return View(data);
            }
            return RedirectToLoginPage;
        }

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
        public ActionResult ChooseOption()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ImportPlayer()
        {
            return View();
        }

        private bool CheckPlayersDoubleEmail(BatchPlayersView batchPlayersView)
        {
            bool result = true;
            var listDoubleEmail = batchPlayersView.Players.GroupBy(p => p.Value.Email).Select(group => new
            {
                Email = group.Key,
                Count = group.Count()
            });
            if (listDoubleEmail.Any(p => p.Count > 1))
            {
                var doubleEmails = listDoubleEmail.Where(p => p.Count > 1).Select(p => p.Email);
                foreach (var player in batchPlayersView.Players)
                {
                    if (doubleEmails.Any(p => p == player.Value.Email))
                    {
                        ModelState.AddModelError("Players[" + player.Key + "].Value.Email", "Email address should be unique");
                        result = false;  
                    }
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0 && System.IO.Path.GetExtension(Request.Files[0].FileName) == ".csv")
            {
                Stream fileStream = Request.Files[0].InputStream;
                using (CsvParser csvParser = new CsvParser(fileStream))
                {
                    BatchPlayersView batchPlayersView = csvParser.Parse();
                    //string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                    //foreach (var player in batchPlayersView.Players)
                    //{
                    //    //check FirstName
                    //    if (String.IsNullOrEmpty(player.Value.FirstName) || String.IsNullOrWhiteSpace(player.Value.FirstName))
                    //    {
                    //        ModelState.AddModelError("Players[" + player.Key + "].Value.FirstName", "Enter first name");
                    //    }
                    //    //check LastName
                    //    if (String.IsNullOrEmpty(player.Value.LastName) || String.IsNullOrWhiteSpace(player.Value.LastName))
                    //    {
                    //        ModelState.AddModelError("Players[" + player.Key + "].Value.LastName", "Enter last name");
                    //    }
                    //    //chek Email for required
                    //    if (String.IsNullOrEmpty(player.Value.Email) || String.IsNullOrWhiteSpace(player.Value.Email))
                    //    {
                    //        ModelState.AddModelError("Players[" + player.Key + "].Value.Email", "Enter Email");
                    //    }
                    //    //check email for correct
                    //    else if (!Regex.IsMatch(player.Value.Email, pattern, RegularExpressions.RegexOptions.Compiled | RegularExpressions.RegexOptions.IgnoreCase))
                    //    {
                    //        ModelState.AddModelError("Players[" + player.Key + "].Value.Email", "Enter correct Email");
                    //    } //check email for already existed 
                    //    else if (Repository.Users.Count(p => string.Compare(p.Email, player.Value.Email, true) == 0) > 0)
                    //    {
                    //        ModelState.AddModelError("Players[" + player.Key + "].Value.Email", "Email already registered");
                    //    }
                    //    CheckPlayersDoubleEmail(batchPlayersView);
                    //}
                    if (batchPlayersView.Players.Count == 0)
                    {
                        ViewBag.TitleError = "DATA NOT FOUND";
                        ViewBag.TextError = "Sorry, but data about players were not found in uploaded file.";
                        return View("ImportError");
                    }
                    else 
                    {
                        return View(batchPlayersView);
                    }
                }//end uding
            }
            else if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0 && System.IO.Path.GetExtension(Request.Files[0].FileName) != ".csv")
            {
                ViewBag.TitleError = "FILE EXTENTION";
                ViewBag.TextError = "Sorry, but the downloaded file is not csv.";
                return View("ImportError");
            }
            else
            {
                ViewBag.TitleError = "DOWNLOAD ERROR";
                ViewBag.TextError = "Sorry, but there was an error when loading the file.";
                return View("ImportError");
            }
        }

        [HttpPost]
        public ActionResult SubmitUploadFile(BatchPlayersView batchPlayersView, bool firstCheck=false)
        {
                    
            CheckPlayersDoubleEmail(batchPlayersView);

            if (firstCheck == true)
            {
                Regex regex = new Regex(@"\[([a-zA-Z0-9]+)\]", RegularExpressions.RegexOptions.Compiled | RegularExpressions.RegexOptions.IgnoreCase);
                HashSet<string> errors = new HashSet<string>(
                    ModelState.AsQueryable()
                    .Where(f => f.Value.Errors.Count > 0)
                    .Select(x => regex.Match(x.Key).Groups[1].Value)
                );

                var unvalidPlayers = batchPlayersView.Players.Where(x => errors.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                var unvalidBatchPlayers = new BatchPlayersView()
                {
                    Players = unvalidPlayers
                };
                var validPlayers = batchPlayersView.Players.Where(x => !errors.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                if (validPlayers.Count > 0)
                {
                    var validBatchPlayers = new BatchPlayersView()
                    {
                        Players = validPlayers
                    };
                    SavePlayers(validBatchPlayers);
                }

                ViewBag.AddPlayersCount = validPlayers.Count;
                if (unvalidPlayers.Count > 0)
                {
                    ViewBag.TotalPlayersCount = batchPlayersView.Players.Count;
                    return View(unvalidBatchPlayers);
                }
                else if (unvalidPlayers.Count == 0)
                {
                    return View("UploadSuccess");
                }
            }
           

            if (batchPlayersView.Players.Count > 0 && ModelState.IsValid)
            {
                SavePlayers(batchPlayersView);
            }
            else if (batchPlayersView.Players.Count > 0 && !ModelState.IsValid)
            {
                return View("SubmitUploadPlayersBody", batchPlayersView);
            }

            return Json(new { result = "success", count = batchPlayersView.Players.Count}, JsonRequestBehavior.AllowGet);
           
        }

        

        [HttpPost]
        public ActionResult UploadSuccess(int count)
        {
            ViewBag.AddPlayersCount = count;
            return View();
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
            CheckPlayersDoubleEmail(batchPlayersView);
            if (ModelState.IsValid)
            {
                if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo && batchPlayersView.Players.Count > 0)
                {
                    Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.AddPlayers);
                }
                SavePlayers(batchPlayersView);
                return View("_OK");
            }
            return View("AddPlayersBody", batchPlayersView);
        }

        private void SavePlayers(BatchPlayersView batchPlayersView)
        {
            foreach (var playerView in batchPlayersView.Players.Values)
            {
                var user = (User)ModelMapper.Map(playerView, typeof(PlayerView), typeof(User));
                user.Password = StringExtension.CreateRandomPassword(8, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                user.PlayerOfTeamID = CurrentUser.OwnTeam.ID;
                user.TutorialStep = 1;
                user.Mode = (int)Model.User.ModeEnum.Tutorial;
                user.IsPhantom = (User.ModeEnum)CurrentUser.Mode == Model.User.ModeEnum.Test;
                Repository.CreateUser(user);

                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 3 //player
                };

                Repository.CreateUserRole(userRole);
                SendWelcomePlayerMail(user.Email, "Welcome to Platform!", CurrentUser.FirstName + " " + CurrentUser.LastName, user.Email, user.Password);
                var existFailMail = Repository.FailedMails.FirstOrDefault(p => string.Compare(p.FailEmail, user.Email, true) == 0);
                if (existFailMail != null)
                {
                    Repository.RemoveFailedMail(existFailMail.ID);
                }
            }
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


        public ActionResult Calendar(DateTime date, DateTime selectedDate)
        {
            var attendanceDateInfo = new AttendanceDateInfo()
            {
                Date = date,
                SelectedDate = selectedDate
            };
            return View(attendanceDateInfo);
        }

        public ActionResult CalendarBody(DateTime date, DateTime selectedDate)
        {
            var attendanceDateInfo = new AttendanceDateInfo()
            {
                Date = date,
                SelectedDate = selectedDate
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
