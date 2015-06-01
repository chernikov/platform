using platformAthletic.Model;
using platformAthletic.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach,individual,player,assistant")]
    public class ReportController : DefaultController
    {
        [Authorize(Roles = "coach,individual,assistant")]
        public ActionResult Index(SearchAttendanceReport search)
        {
            if (search == null)
            {
                search = new SearchAttendanceReport();
            }

            var attendanceReport = new AttendanceReport(search, CurrentUser.OwnTeam);
            return View(attendanceReport);
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


        public ActionResult Workouts(int id)
        {
            var user = Repository.TeamPlayersUsers.FirstOrDefault(p => p.ID == id);
            if (user == null)
            {
                return null;
            }
            return View(user);
        }

        public ActionResult AttendanceCalendar(int id)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var attendanceInfo = new AttendanceInfo()
                {
                    User = user,
                    Date = DateTime.Now.Current(),
                };
                return View(attendanceInfo);
            }
            return null;
        }

        public ActionResult AttendanceCalendarBody(int id, DateTime date)
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

        public ActionResult Progress(SearchProgressReport search)
        {
            if (search == null)
            {
                search = new SearchProgressReport();
            }

            var progressReport = new ProgressReport(search, CurrentUser.OwnTeam);
            return View(progressReport);
        }

        public ActionResult ProgressSummary(int id, DateTime startDate, DateTime endDate)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var progressInfo = new ProgressInfo(user, startDate, endDate);
                
                return View(progressInfo);
            }
            return null;
        }

        public ActionResult ProgressGraph(int id, DateTime startDate, DateTime endDate)
        {
            var user = CurrentUser.OwnTeam.ActiveUsers.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var progressInfo = new ProgressGraphInfo()
                {
                    User = user,
                    StartDate = startDate,
                    EndDate = endDate
                };
                return View(progressInfo);
            }
            return null;
        }

        public ActionResult Performance(int id,DateTime startDate, DateTime endDate)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var sunday = endDate.AddDays(-(int)endDate.DayOfWeek);
                var currentSunday = startDate.AddDays(-(int)startDate.DayOfWeek);
                var labels = new List<string>();
                var sData = new List<int>();
                var bData = new List<int>();
                var cData = new List<int>();
                var tData = new List<int>();
                while (currentSunday <= sunday)
                {
                    var sbc = user.SBCHistory(currentSunday);
                    if (sbc != null)
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                        tData.Add((int)(sbc.Squat + sbc.Bench + sbc.Clean));
                    }
                    currentSunday = currentSunday.AddDays(7);
                }
                var datasets = new List<PerformanceGraphInfo>();
                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Squat",
                    fillColor = "transparent",
                    strokeColor = "#ed4848",
                    pointColor = "#ed4848",
                    pointStrokeColor = "#ed4848",
                    pointHighlightFill = "#ed4848",
                    pointHighlightStroke = "#ed4848",
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
                    pointHighlightFill = "#3bcb67",
                    pointHighlightStroke = "#3bcb67",
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
                    pointHighlightFill = "#60b1c2",
                    pointHighlightStroke = "#60b1c2",
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
                    pointHighlightFill = "#495b6c",
                    pointHighlightStroke = "#495b6c",
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
        }


        //public ActionResult FillWeekAttendance()
        //{
        //    var users = Repository.TeamPlayersUsers.ToList();

        //    var rand = new Random((int)DateTime.Now.Current().Ticks);

        //    var j = 0;
        //    foreach (var user in users)
        //    {
        //        j++;
        //        var value = rand.Next(16);
        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (value % 2 == 1)
        //            {
        //                Repository.CreateUserAttendance(new UserAttendance()
        //                {
        //                    UserID = user.ID,
        //                    UserSeasonID = user.CurrentSeason.ID,
        //                    AddedDate = DateTime.Now.Current().AddDays(-(int)DateTime.Now.DayOfWeek + i)
        //                });
        //            }
        //            value = value >> 1;
        //        }
        //    }
        //    return null;
        //}
    }
}
