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
    [Authorize(Roles = "coach,player,assistant")]
    public class ReportController : DefaultController
    {
        [Authorize(Roles = "coach,player,assistant")]
        public ActionResult Index(SearchAttendanceReport search)
        {
            if (search == null)
            {
                search = new SearchAttendanceReport();
            }

            var attendanceReport = new AttendanceReport(search, CurrentUser.OwnTeam ?? CurrentUser.Team);
            return View(attendanceReport);
        }

        public ActionResult JsonPlayers()
        {
            var team = CurrentUser.OwnTeam ?? CurrentUser.Team;

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

        public ActionResult AttendanceCalendar(int id)
        {
            var team = CurrentUser.OwnTeam ?? CurrentUser.Team;

            var user = team.ActiveUsers.FirstOrDefault(p => p.ID == id);
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
            var team = CurrentUser.OwnTeam ?? CurrentUser.Team;
            var user = team.ActiveUsers.FirstOrDefault(p => p.ID == id);
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

            var progressReport = new ProgressReport(search, CurrentUser.OwnTeam ?? CurrentUser.Team);
            return View(progressReport);
        }

        public ActionResult ProgressGraph(int id, DateTime startDate, DateTime endDate)
        {
            var team = CurrentUser.OwnTeam ?? CurrentUser.Team;
            var user = team.ActiveUsers.FirstOrDefault(p => p.ID == id);
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

        public ActionResult ProgressTeamGraph(SearchProgressReport search)
        {
            search.TeamID = (CurrentUser.OwnTeam ?? CurrentUser.Team).ID;
            return View(search);
        }

        public ActionResult Performance(int id, DateTime startDate, DateTime endDate)
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
                while (currentSunday <= sunday)
                {
                    var sbc = user.SBCHistory(currentSunday);
                    if (sbc != null)
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                    }
                    else
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add(0);
                        bData.Add(0);
                        cData.Add(0);
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
                var data = new
                {
                    labels,
                    datasets
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult Performance30(int id, DateTime startDate, DateTime endDate)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var month = endDate.AddDays(-(int)endDate.Day).AddMonths(1);
                var currentMonth = startDate.AddDays(-(int)startDate.Day).AddDays(1).AddMonths(-1);
                var labels = new List<string>();
                var sData = new List<int>();
                var bData = new List<int>();
                var cData = new List<int>();
                while (currentMonth <= month)
                {
                    var sbc = user.SBCHistory(currentMonth);
                    if (sbc != null)
                    {
                        labels.Add(currentMonth.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                    }
                    else
                    {
                        labels.Add(currentMonth.ToString("MMM/dd"));
                        sData.Add(0);
                        bData.Add(0);
                        cData.Add(0);
                    }
                    currentMonth = currentMonth.AddMonths(1);
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
                var data = new
                {
                    labels,
                    datasets
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}
