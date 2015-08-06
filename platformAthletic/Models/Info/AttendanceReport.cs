using platformAthletic.Model;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;

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

        public Record WorkoutComplete { get; set; }

        private Team _team;

        public AttendanceReport(SearchAttendanceReport search, Team team)
        {
           
            var profiler = MiniProfiler.Current; // it's ok if this is null
            var zeroDay = new DateTime(1970, 1, 1);
            if (!search.StartPeriod.HasValue || search.StartPeriod.Value < zeroDay)
            {
                search.StartPeriod = team.User.AddedDate;
            }
            if (!search.EndPeriod.HasValue || search.EndPeriod.Value < zeroDay)
            {
                search.EndPeriod = DateTime.Now.Current();
            }

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
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Process"))
            {
                var users = Repository.TeamPlayersAndPhantomUsers.Where(p => p.PlayerOfTeamID == Search.TeamID);

                using (profiler.Step("Filter"))
                {
                    Filter(ref users);
                }
                using (profiler.Step("Fill"))
                {
                    var startYear = new DateTime(SqlSingleton.sqlRepository.CurrentDateTime.Year, 1, 1);
                    var startMonth = new DateTime(SqlSingleton.sqlRepository.CurrentDateTime.Year, SqlSingleton.sqlRepository.CurrentDateTime.Month, 1);
                    int diff = SqlSingleton.sqlRepository.CurrentDateTime.DayOfWeek - DayOfWeek.Sunday;
                    if (diff < 0)
                    {
                        diff += 7;
                    }
                    var startWeek = SqlSingleton.sqlRepository.CurrentDateTime.AddDays(-1 * diff).Date;
                    foreach (var user in users)
                    {
                        var attendances = Repository.UserAttendances.Where(p => p.UserID == user.ID);

                        var allTime = 0;
                        if (Search.StartPeriod.HasValue || Search.EndPeriod.HasValue)
                        {
                            var selectedAttendances = attendances;
                            if (Search.StartPeriod.HasValue)
                            {
                                selectedAttendances = selectedAttendances.Where(p => p.AddedDate >= Search.StartPeriod.Value);
                            }
                            if (Search.EndPeriod.HasValue)
                            {
                                selectedAttendances = selectedAttendances.Where(p => p.AddedDate < Search.EndPeriod.Value);
                            }
                            allTime = selectedAttendances.Count();
                        }
                        else
                        {
                            allTime = attendances.Count();
                        }

                        var year = attendances.Count(p => p.AddedDate >= startYear);
                        var month = attendances.Count(p => p.AddedDate >= startMonth);
                        var week = attendances.Count(p => p.AddedDate >= startWeek);
                        List.Add(new Record()
                        {
                            User = user,
                            Week = week,
                            Month = month,
                            Year = year,
                            AllTime = allTime,
                        });
                    }
                }
                List = List.OrderByDescending(p => p.AllTime).ToList();
                using (profiler.Step("SetRanks"))
                {
                    SetRanks();
                }
                using (profiler.Step("Order"))
                {
                    Order();
                }
                if (Search.StartID.HasValue)
                {
                    var item = List.FirstOrDefault(p => p.User.ID == Search.StartID.Value);
                    if (item != null)
                    {
                        var index = List.IndexOf(item);
                        Search.Page = index / 20 + 1;
                    }
                }
                WorkoutComplete = new Record()
                {
                    AllTime = List.Sum(p => p.AllTime),
                    Year = List.Sum(p => p.Year),
                    Month = List.Sum(p => p.Month),
                    Week = List.Sum(p => p.Week)
                };
                using (profiler.Step("GetPage"))
                {
                    GetPage();
                }
                if (!Search.StartID.HasValue && List.Count > 0)
                {
                    Search.StartID = List[0].User.ID;
                }
            }
        }

        protected void Order()
        {
            switch (Search.Sort)
            {
                case SearchAttendanceReport.SortEnum.AllTimeAsc:
                    List = List.OrderBy(p => p.AllTime).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.AllTimeDesc:
                    List = List.OrderByDescending(p => p.AllTime).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.YearAsc:
                    List = List.OrderBy(p => p.Year).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.YearDesc:
                    List = List.OrderByDescending(p => p.Year).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.MonthAsc:
                    List = List.OrderBy(p => p.Month).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.MonthDesc:
                    List = List.OrderByDescending(p => p.Month).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.WeekAsc:
                    List = List.OrderBy(p => p.Week).ToList();
                    break;
                case SearchAttendanceReport.SortEnum.WeekDesc:
                    List = List.OrderByDescending(p => p.Week).ToList();
                    break;
                default:
                    List = List.OrderByDescending(p => p.AllTime).ToList();
                    break;
            }
        }

        protected void Filter(ref IQueryable<User> users)
        {
            if (Search.SportID != null)
            {
                users = users.Where(p => p.UserFieldPositions.Any(r => r.SportID == Search.SportID));
            }
            if (Search.FieldPositionID != null)
            {
                users = users.Where(p => p.UserFieldPositions.Any(r => r.FieldPositionID == Search.FieldPositionID));
            }
            if (Search.GroupID != null)
            {
                users = users.Where(p => p.GroupID == Search.GroupID);
            }
            if (Search.GradYear != null)
            {
                users = users.Where(p => p.GradYear == Search.GradYear);
            }
        }

        private void SetRanks()
        {
            TotalCount = List.Count();
            var i = 1;
            var rank = 0;
            int rankValue = -1;
            int currentValue = 0;
            foreach (var record in List)
            {
                currentValue = GetRankValue(record, Search.Sort);
                if (currentValue != rankValue)
                {
                    rank = i;
                    rankValue = currentValue;
                }
                record.Position = rank;
                i++;
            }

        }

        protected int GetRankValue(Record record, SearchAttendanceReport.SortEnum sort)
        {
            return record.AllTime;
            //switch (Search.Sort)
            //{
                    
            //    case SearchAttendanceReport.SortEnum.AllTimeAsc:
            //    case SearchAttendanceReport.SortEnum.AllTimeDesc:
            //        return record.AllTime;
            //    case SearchAttendanceReport.SortEnum.YearAsc:
            //    case SearchAttendanceReport.SortEnum.YearDesc:
            //        return record.Year;
            //    case SearchAttendanceReport.SortEnum.MonthAsc:
            //    case SearchAttendanceReport.SortEnum.MonthDesc:
            //        return record.Month;
            //    case SearchAttendanceReport.SortEnum.WeekAsc:
            //    case SearchAttendanceReport.SortEnum.WeekDesc:
            //        return record.Week;
            //    default:
            //        return record.AllTime;
            //}
        }

        protected virtual void GetPage()
        {
            List = List.Skip((Search.Page - 1) * 20).Take(20).ToList();
        }
    }
}