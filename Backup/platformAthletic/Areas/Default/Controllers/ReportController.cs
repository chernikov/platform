using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.Info;
using platformAthletic.Helpers;
using platformAthletic.Model;
using platformAthletic.Tools;
using platformAthletic.Attributes;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach,individual,player")]
    public class ReportController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index()
        {
            var commonReport = new CommonReport(CurrentUser, Repository);
            if (double.IsNaN(commonReport.Squat))
            {
                commonReport.Squat = 0;
                commonReport.Bench = 0;
                commonReport.Clean = 0;
            }
            if (CurrentUser.InRoles("coach"))
            {
                return View(commonReport);
            }
            if (CurrentUser.InRoles("individual,player"))
            {
                return View("Individual", commonReport);
            }
            return RedirectToLoginPage;
        }

        [Authorize(Roles = "coach")]
        public ActionResult Attendance(FilterCustomAttendanceReport filterCustomAttendanceReport)
        {
            if (filterCustomAttendanceReport.TeamID == 0)
            {
                filterCustomAttendanceReport = new FilterCustomAttendanceReport
                {
                    TeamID = CurrentUser.OwnTeam.ID,
                    BeginDate = CurrentUser.CurrentSeason.StartDay,
                    EndDate = DateTime.Now
                };
            };
            return View(filterCustomAttendanceReport);
        }

        public ActionResult AttendanceChart(FilterCustomAttendanceReport filterCustomAttendanceReport)
        {
            return View(filterCustomAttendanceReport);
        }

        public ActionResult GetAttendanceData(FilterCustomAttendanceReport filterCustomAttendanceReport)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == filterCustomAttendanceReport.TeamID);
            if (team != null)
            {
                var reportEndDate = DateTime.Now.Date;
                reportEndDate = reportEndDate.AddDays(-(int)reportEndDate.DayOfWeek);
                if (reportEndDate > team.CurrentSeason.StartDay.AddDays(team.CurrentSeason.Season.DaysLength))
                {
                    reportEndDate = team.CurrentSeason.StartDay.AddDays(team.CurrentSeason.Season.DaysLength);
                }
                Dictionary<string, double> output = null;
                output = GetAttendance(team, filterCustomAttendanceReport);
                  
                var data = output.Select(p => new { p.Key, p.Value }).ToArray();
                return Json(new
                {
                    result = "ok",
                    data
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public Dictionary<string, double> GetAttendance(Team team, FilterCustomAttendanceReport filter)
        {
            var output = new Dictionary<string, double>();

            switch (filter.SelectedType)
            {
                case FilterCustomAttendanceReport.Type.Team:
                    GenerateAttendance(team.Users, team.Name, filter.BeginDate, filter.EndDate, output);
                    break;
                case FilterCustomAttendanceReport.Type.Group:
                    foreach (var group in Repository.Groups.Where(p => p.TeamID == team.ID).ToList())
                    {
                        var users = team.Users.Where(p => p.GroupID == group.ID);
                        GenerateAttendance(users, group.Name, filter.BeginDate, filter.EndDate, output);
                    }
                    break;
                case FilterCustomAttendanceReport.Type.Position:
                    foreach (var fieldPosition in Repository.FieldPositions.ToList())
                    {
                        var users = team.Users.Where(p => p.FieldPositions.Any(r => r.ID == fieldPosition.ID));
                        GenerateAttendance(users, fieldPosition.Name, filter.BeginDate, filter.EndDate, output);
                    }
                    break;
                case FilterCustomAttendanceReport.Type.Individual:
                    foreach (var user in team.Users.OrderBy(p => p.LastName).ToList())
                    {
                        var users = team.Users.Where(p => p.ID == user.ID);
                        GenerateAttendance(users, user.LastName + ", " + user.FirstName, filter.BeginDate, filter.EndDate, output);
                    }

                    break;
            }
            if (!output.Any())
            {
                output.Add("Nobody", 0);
            }
            return output;
        }

        private static void GenerateAttendance(IEnumerable<User> usersSource, string Name, DateTime startDate, DateTime endDate, Dictionary<string, double> output)
        {
            int totalAttendance = 0;
            int totalCount = 0;
            var beginReportDay = endDate.AddDays(-7);
            var endReportDay = endDate;
            while (beginReportDay >= startDate)
            {
                foreach (var user in usersSource)
                {
                    if (user.AddedDate.Date <= beginReportDay)
                    {
                        totalCount += 3;
                        var attendance = user.UserAttendances.Count(p => p.AddedDate >= beginReportDay && p.AddedDate <= endReportDay);
                        if (attendance > 3)
                        {
                            attendance = 3;
                        }
                        totalAttendance += attendance;
                    }
                }
                endReportDay = beginReportDay;
                beginReportDay = beginReportDay.AddDays(-7);
            }
            if (totalCount > 0)
            {
                output.Add(Name, (double)((int)(totalAttendance * 100 / totalCount)) / 100);
            }
            else
            {
                output.Add(Name, 0);
            }
        }

        public ActionResult Progress(FilterCustomProgressReport filterCustomProgressReport)
        {
            if (CurrentUser.InRoles("coach"))
            {
                if (filterCustomProgressReport.TeamID == 0)
                {
                    filterCustomProgressReport = new FilterCustomProgressReport
                    {
                        TeamID = CurrentUser.OwnTeam.ID,
                        EndDate = DateTime.Now,
                        BeginDate = CurrentUser.FullProgressStartDate ?? CurrentUser.CurrentSeason.StartDay
                    };
                };
                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Individual)
                {
                    if (filterCustomProgressReport.Users.Any())
                    {
                        filterCustomProgressReport.UserID = filterCustomProgressReport.Users.First().ID;
                    }
                    else
                    {
                        filterCustomProgressReport.UserID = 0;
                    }
                }

                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Group)
                {
                    if (filterCustomProgressReport.Groups.Any())
                    {
                        filterCustomProgressReport.GroupID = filterCustomProgressReport.Groups.First().ID;
                    }
                    else
                    {
                        filterCustomProgressReport.GroupID = null;
                    }
                }
                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Position)
                {
                    filterCustomProgressReport.FieldPositionID = filterCustomProgressReport.FieldPositions.First().ID;
                }
                return View(filterCustomProgressReport);
            }
            if (CurrentUser.InRoles("individual,player"))
            {
                filterCustomProgressReport = new FilterCustomProgressReport
                {
                    EndDate = DateTime.Now,
                    BeginDate = CurrentUser.FullProgressStartDate ?? CurrentUser.CurrentSeason.StartDay 
                };
                return View("IndividualProgress", filterCustomProgressReport);
            }
            return RedirectToLoginPage;
        }

        public ActionResult ProgressChart(FilterCustomProgressReport filterCustomProgressReport)
        {
            if (CurrentUser.InRoles("individual,player"))
            {
                return View("IndividualProgressChart", filterCustomProgressReport);
            }
            return View(filterCustomProgressReport);
        }

        public ActionResult GetProgressData(FilterCustomProgressReport filterCustomProgressReport)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == filterCustomProgressReport.TeamID);
            if (team != null)
            {
                var output = new Dictionary<DateTime, double>();

                var currentDate = filterCustomProgressReport.BeginDate;
                var endDate = filterCustomProgressReport.EndDate;
                while (currentDate < endDate)
                {
                    var value = GetUserWeightData(team, filterCustomProgressReport, currentDate);
                    output.Add(currentDate, double.IsNaN(value) ? 0 : value);
                    switch (filterCustomProgressReport.SelectedPeriod)
                    {
                        case FilterCustomProgressReport.Period.ByWeek:
                            currentDate = currentDate.AddDays(7);
                            break;
                        case FilterCustomProgressReport.Period.ByMonth:
                            currentDate = currentDate.AddMonths(1);
                            break;
                    }
                }
                if (!output.Any())
                {
                    output.Add(DateTime.Now, 0);
                }
                var data = output.Select(p => new { 
                    p.Key, 
                    Value = Math.Round(p.Value, 0, MidpointRounding.ToEven) }).ToArray();
                return Json(new
                {
                    result = "ok",
                    data
                }, JsonRequestBehavior.AllowGet);
            }
            else if (CurrentUser.InRoles("individual,player"))
            {

                var output = new Dictionary<DateTime, double>();
                var currentDate = filterCustomProgressReport.BeginDate;
                var endDate = filterCustomProgressReport.EndDate;
                while (currentDate < endDate)
                {
                    var value = GetUserValue(CurrentUser, filterCustomProgressReport.Weight, currentDate);
                    if (value.HasValue)
                    {
                        output.Add(currentDate, value.Value);
                    }
                    switch (filterCustomProgressReport.SelectedPeriod)
                    {
                        case FilterCustomProgressReport.Period.ByWeek:
                            currentDate = currentDate.AddDays(7);
                            break;
                        case FilterCustomProgressReport.Period.ByMonth:
                            currentDate = currentDate.AddMonths(1);
                            break;
                    }
                }
                var data = output.Select(p => new { p.Key, p.Value }).ToArray();
                return Json(new
                {
                    result = "ok",
                    data
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "error" });
        }

        private double GetUserWeightData(Team team, FilterCustomProgressReport filter, DateTime beginDate)
        {
            double total = 0;
            int count = 0;

            switch (filter.SelectedType)
            {
                case FilterCustomProgressReport.Type.Team:
                    foreach (var user in team.Users.ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Group:
                    foreach (var user in team.Users.Where(p => p.GroupID == filter.GroupID).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Individual:
                    foreach (var user in team.Users.Where(p => p.ID == filter.UserID).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Position:
                    foreach (var user in team.Users.Where(p => p.FieldPositions.Any(r => r.ID == filter.FieldPositionID)).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
            }
            return total / count;
        }

        private double? GetUserValue(User user, FilterCustomProgressReport.WeightEnum weight, DateTime beginDate)
        {
            var record = user.SBCValues.Where(p => p.AddedDate <= beginDate).OrderByDescending(p => p.ID).FirstOrDefault();
            if (record != null)
            {
                switch (weight)
                {
                    case FilterCustomProgressReport.WeightEnum.All:
                        return record.Clean + record.Squat + record.Bench;
                    case FilterCustomProgressReport.WeightEnum.Squat:
                        return record.Squat;
                    case FilterCustomProgressReport.WeightEnum.Bench:
                        return record.Bench;
                    case FilterCustomProgressReport.WeightEnum.Clean:
                        return record.Clean;
                }
            }
            return null;
        }

        [Authorize(Roles = "coach")]
        public ActionResult AttendancePrint(FilterCustomAttendanceReport filterCustomAttendanceReport)
        {
            if (filterCustomAttendanceReport.TeamID == 0)
            {
                filterCustomAttendanceReport = new FilterCustomAttendanceReport
                {
                    TeamID = CurrentUser.OwnTeam.ID
                };
            };
            return View(filterCustomAttendanceReport);
        }

        public ActionResult ProgressPrint(FilterCustomProgressReport filterCustomProgressReport)
        {

            if (CurrentUser.InRoles("coach"))
            {
                if (filterCustomProgressReport.TeamID == 0)
                {
                    filterCustomProgressReport = new FilterCustomProgressReport
                    {
                        TeamID = CurrentUser.OwnTeam.ID
                    };
                };
                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Individual)
                {
                    if (filterCustomProgressReport.UserID == 0) {
                    filterCustomProgressReport.UserID = filterCustomProgressReport.Users.First().ID;
                    }
                }
                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Group)
                {
                    if (!filterCustomProgressReport.GroupID.HasValue)
                    {
                        filterCustomProgressReport.GroupID = filterCustomProgressReport.Groups.First().ID;
                    }
                }
                if (filterCustomProgressReport.SelectedType == FilterCustomProgressReport.Type.Position)
                {
                    if (filterCustomProgressReport.FieldPositionID == 0)
                    {
                        filterCustomProgressReport.FieldPositionID = filterCustomProgressReport.FieldPositions.First().ID;
                    }
                }
                return View(filterCustomProgressReport);
            }
            if (CurrentUser.InRoles("individual,player"))
            {
                return View("IndividualProgress", filterCustomProgressReport);
            }
            return RedirectToLoginPage;
        }

        [Authorize(Roles = "coach,individual")]
        public ActionResult ResetAttendance()
        {
            Repository.ResetAttendance(CurrentUser);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "coach,individual")]
        public ActionResult ResetProgress()
        {
            Repository.ResetProgress(CurrentUser);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}
