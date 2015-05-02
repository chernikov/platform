using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;
using platformAthletic.Helpers;

namespace platformAthletic.Models.Info
{
    public class CommonReport
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public double Squat { get; set; }

        public double Bench { get; set; }

        public double Clean { get; set; }

        /// <summary>
        /// Percentage
        /// </summary>
        public int MonthAttendance { get; set; }

        /// <summary>
        /// Percentage
        /// </summary>
        public int YearToDateAttendance { get; set; }

        public Team Team { get; set; }

        public User User { get; set; }

        public CommonReport(User user, IRepository repository)
        {
            if (user.InRoles("coach"))
            {
                if (!user.ProgressStartDate.HasValue)
                {
                    var first = user.OwnTeam.SBCValues.OrderBy(p => p.ID).FirstOrDefault(p => p.Squat != 0 && p.Clean != 0 && p.Bench != 0);
                    if (first != null)
                    {
                        user.ProgressStartDate = first.AddedDate;
                        repository.SetProgressDate(user);
                    }
                }
                GenerateCoachTypeReport(user);
            }
            if (user.InRoles("player"))
            {
                User = user;
                var coach = user.Team.User;
                var startDate = coach.ProgressStartDate;
                if (!user.ProgressStartDate.HasValue)
                {
                    var first = user.SBCValues.OrderBy(p => p.ID).FirstOrDefault(p => p.Squat != 0 && p.Clean != 0 && p.Bench != 0);
                    if (first != null)
                    {
                        startDate = first.AddedDate;
                    }
                }
                GenerateProgress(new List<User> { user }, user.CurrentSeason, startDate);
            }
            if (user.InRoles("individual"))
            {
                User = user;
                if (!user.ProgressStartDate.HasValue)
                {
                    var first = user.SBCValues.OrderBy(p => p.ID).FirstOrDefault(p => p.Squat != 0 && p.Clean != 0 && p.Bench != 0);
                    user.ProgressStartDate = first.AddedDate;
                    repository.SetProgressDate(user);
                }
                GenerateProgress(new List<User> { user }, user.CurrentSeason, user.ProgressStartDate);
            }
            
        }

        private void GenerateCoachTypeReport(User coach)
        {
            Team = coach.OwnTeam;
            if (coach.CurrentSeason != null)
            {
                GenerateProgress(Team.Users.ToList(), coach.CurrentSeason, coach.ProgressStartDate);
                GenerateMonthAttendance(Team.Users.ToList(), coach.CurrentSeason, coach.AttendanceStartDate);
                GenerateTotalAttendance(Team.Users.ToList(), coach.CurrentSeason, coach.AttendanceStartDate);
            }
        }

        private void GenerateProgress(List<User> users, UserSeason userSeason, DateTime? initialStartDate = null)
        {
            double totalBeginSquat = 0;
            double totalBeginBench = 0;
            double totalBeginClean = 0;

            int totalBeginCount = 0;

            double totalEndSquat = 0;
            double totalEndBench = 0;
            double totalEndClean = 0;

            int totalEndCount = 0;

            var startDate = userSeason.StartDay.AddDays(1).Date;
            if (initialStartDate.HasValue)
            {
                startDate = initialStartDate.Value;
            }
            var endDate = DateTime.Now.Current().AddDays(1).Date;

            foreach (var user in users)
            {
                var sbcFirst = user.SBCValues.Where(p => p.AddedDate <= startDate).OrderByDescending(p => p.ID).FirstOrDefault();

                if (sbcFirst != null)
                {
                    totalBeginCount++;
                    totalBeginSquat += sbcFirst.Squat;
                    totalBeginBench += sbcFirst.Bench;
                    totalBeginClean += sbcFirst.Clean;
                }
                var sbcEnd = user.SBCValues.Where(p => p.AddedDate <= endDate).OrderByDescending(p => p.ID).FirstOrDefault();

                if (sbcEnd != null)
                {
                    totalEndCount++;
                    totalEndSquat += sbcEnd.Squat;
                    totalEndBench += sbcEnd.Bench;
                    totalEndClean += sbcEnd.Clean;
                }
            }

            if (totalBeginCount == 0)
            {
                foreach (var user in users)
                {
                    var sbcFirst = user.SBCValues.Where(p => p.AddedDate >= startDate).OrderBy(p => p.ID).FirstOrDefault();

                    if (sbcFirst != null)
                    {
                        totalBeginCount++;
                        totalBeginSquat += sbcFirst.Squat;
                        totalBeginBench += sbcFirst.Bench;
                        totalBeginClean += sbcFirst.Clean;
                    }
                }
            }

            Squat = (totalEndSquat / totalEndCount) - (totalBeginSquat / totalBeginCount);
            Bench = (totalEndBench / totalEndCount) - (totalBeginBench / totalBeginCount);
            Clean = (totalEndClean / totalEndCount) - (totalBeginClean / totalBeginCount);
        }

        private void GenerateMonthAttendance(List<User> users, UserSeason userSeason, DateTime? initialStartDate = null)
        {
            var reportEndDate = DateTime.Now.Current().Date;
            reportEndDate = reportEndDate.AddDays(-(int)reportEndDate.DayOfWeek);

            if (reportEndDate > userSeason.StartDay.AddDays(userSeason.Season.DaysLength))
            {
                reportEndDate = userSeason.StartDay.AddDays(userSeason.Season.DaysLength);
            }

            int totalAttendance = 0;
			int totalCount = 0;
            var beginReportDay = reportEndDate.AddDays(-7);
            var endReportDay = reportEndDate;
            for (int i = 0; i < 4; i++)
            {
                if (initialStartDate.HasValue && beginReportDay < initialStartDate.Value)
                {
                    break;
                }
                foreach (var user in users)
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
                MonthAttendance = totalAttendance * 100 / totalCount;
            }
        }

        private void GenerateTotalAttendance(List<User> users, UserSeason userSeason, DateTime? initialStartDate = null)
        {
            var reportEndDate = DateTime.Now.Current().Date;
			
            reportEndDate = reportEndDate.AddDays(-(int)reportEndDate.DayOfWeek);

            if (reportEndDate > userSeason.StartDay.AddDays(userSeason.Season.DaysLength)) 
            {
                reportEndDate = userSeason.StartDay.AddDays(userSeason.Season.DaysLength);
            }

            int totalAttendance = 0;
            int totalCount = 0;
            var beginReportDay = reportEndDate.AddDays(-7);
            var endReportDay = reportEndDate;
            var startDate = initialStartDate ?? userSeason.StartDay;
            while (beginReportDay >= startDate)
            {
                foreach (var user in users)
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
                YearToDateAttendance = totalAttendance * 100 / totalCount;
            }
        }
    }
}