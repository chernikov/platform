using platformAthletic.Model;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class AttendanceReport
    {
        public class Record
        {
            public int Position { get; set; }

            public User User { get; set; }

            public int Week { get; set; }

            public int Month { get; set; }

            public int Year { get; set; }

            public int AllTime { get; set; }

            public int SelectedPeriod { get; set; }
        }

        protected IRepository Repository = DependencyResolver.Current.GetService<IRepository>();

        public SearchAttendanceReport Search { get; set; }

        public List<Record> List { get; set; }

        public int TotalCount { get; set; }

        public int CountPage
        {
            get
            {
                return (int)decimal.Remainder(TotalCount, SearchNationalLeaderboard.PageSize) == 0 ? TotalCount / SearchNationalLeaderboard.PageSize : TotalCount / SearchNationalLeaderboard.PageSize + 1;
            }
        }

        public AttendanceReport(SearchAttendanceReport search, Team team)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Calc Attendance Report"))
            {
                List = new List<Record>();

                search.TeamID = team.ID;
                Search = search;
                Search.Init();
                Process();
            }
        }

        public void Process()
        {
            var users = Repository.TeamPlayersUsers.Where(p => p.PlayerOfTeamID == Search.TeamID);

            foreach (var user in users)
            {
                List.Add(new Record()
                {
                    User = user,
                    Week = user.WeekAttendanceCount,
                    Month = user.MonthAttendanceCount,
                    Year = user.YearAttendanceCount,
                    AllTime = user.AllAttendanceCount,
                });
            }
        }
    }
}