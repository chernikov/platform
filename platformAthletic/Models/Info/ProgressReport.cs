using platformAthletic.Model;
using platformAthletic.Helpers;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class ProgressReport
    {
        public class Record
        {
            public int? TeamID { get; set; }

            public int Position { get; set; }

            public User User { get; set; }

            public int Squat { get; set; }

            public int Bench { get; set; }

            public int Clean { get; set; }

            public int Total
            {
                get
                {
                    return Squat + Bench + Clean;
                }
            }
        }

        protected IRepository Repository = DependencyResolver.Current.GetService<IRepository>();

        public SearchProgressReport Search { get; set; }

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

        public ProgressReport(SearchProgressReport search, Team team)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            var zeroDay = new DateTime(1970, 1, 1);
            if (!search.StartPeriod.HasValue || search.StartPeriod.Value < zeroDay)
            {
                if (team.User.InTestMode && team.User.Role.ToLower() == "coach")
                {
                    search.StartPeriod = team.User.AddedDate.AddDays(-90);
                }
                else
                {
                    search.StartPeriod = team.User.AddedDate;
                }
            }
            if (!search.EndPeriod.HasValue || search.EndPeriod.Value < zeroDay)
            {
                //search.EndPeriod = DateTime.Now.Current();
                //search.EndPeriod = DateTime.Now.Current().Date;
                search.EndPeriod = DateTime.Now;
            }

            using (profiler.Step("Calc Progress Report"))
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
                using (profiler.Step("Search Date"))
                {
                    FilterDateRange(users);
                }
                if (Search.StartPeriod != null)
                {
                    using (profiler.Step("Fill"))
                    {
                        var startDay = Search.StartPeriod.Value.Date;
                        var endDate = (Search.EndPeriod ?? DateTime.Now.Current()).Date;
                        foreach (var user in users)
                        {
                            var sbcValueSquatStart = user.SBCForward(startDay, endDate, SBCValue.SbcType.Squat);
                            var sbcValueBenchStart = user.SBCForward(startDay, endDate, SBCValue.SbcType.Bench);
                            var sbcValueCleanStart = user.SBCForward(startDay, endDate, SBCValue.SbcType.Clean);
                            //if (sbcValueSquatStart == null)
                            //{
                            //    sbcValueSquatStart = new SBCValue();// user.SBCFirstHistory(SBCValue.SbcType.Squat);
                            //}
                            //if (sbcValueBenchStart == null)
                            //{
                            //    sbcValueBenchStart = new SBCValue(); // user.SBCFirstHistory(SBCValue.SbcType.Bench);
                            //}
                            //if (sbcValueCleanStart == null)
                            //{
                            //    sbcValueCleanStart = new SBCValue();  // user.SBCFirstHistory(SBCValue.SbcType.Bench);
                            //}

                            var sbcValueEnd = user.SBCHistory(endDate);
                            if (sbcValueEnd == null)
                            {
                                sbcValueEnd = new SBCValue();
                            }
                      
                            var record = new Record()
                            {
                                User = user
                            };

                            if (sbcValueSquatStart != null)
                            {
                                record.Squat = (int)(sbcValueEnd.Squat - sbcValueSquatStart.Squat);
                            }

                            if (sbcValueBenchStart != null)
                            {
                                record.Bench = (int)(sbcValueEnd.Bench - sbcValueBenchStart.Bench);
                            }

                            if (sbcValueCleanStart != null)
                            {
                                record.Clean = (int)(sbcValueEnd.Clean - sbcValueCleanStart.Clean);
                            }

                            List.Add(record);
                        }
                    }
                }
                List = List.OrderByDescending(p => p.Total).ToList();

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
                    Bench = List.Sum(p => p.Bench),
                    Clean = List.Sum(p => p.Clean),
                    Squat = List.Sum(p => p.Squat),
                    TeamID = Search.TeamID
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

        private void FilterDateRange(IQueryable<User> users)
        {
            var listIDs = users.Select(p => p.ID).ToList();
            var firstSbc = Repository.SBCValues.Where(p => listIDs.Contains(p.UserID)).OrderBy(p => p.AddedDate).FirstOrDefault();
            if (firstSbc != null)
            {
                if (!Search.StartPeriod.HasValue || Search.StartPeriod < firstSbc.AddedDate)
                {
                    Search.StartPeriod = firstSbc.AddedDate;
                }
            }
            if (!Search.EndPeriod.HasValue)
            {
                Search.EndPeriod = DateTime.Now.Current();
            }
            //var lastSbc = Repository.SBCValues.Where(p => listIDs.Contains(p.UserID)).OrderByDescending(p => p.AddedDate).FirstOrDefault();
            //if (lastSbc != null)
            //{
            //    if (!Search.EndPeriod.HasValue || Search.EndPeriod > lastSbc.AddedDate)
            //    {
            //        Search.EndPeriod = lastSbc.AddedDate;
            //    }
            //}
        }

        protected void Order()
        {
            switch (Search.Sort)
            {
                case SearchProgressReport.SortEnum.TotalAsc:
                    List = List.OrderBy(p => p.Total).ToList();
                    break;
                case SearchProgressReport.SortEnum.TotalDesc:
                    List = List.OrderByDescending(p => p.Total).ToList();
                    break;
                case SearchProgressReport.SortEnum.SquatAsc:
                    List = List.OrderBy(p => p.Squat).ToList();
                    break;
                case SearchProgressReport.SortEnum.SquatDesc:
                    List = List.OrderByDescending(p => p.Squat).ToList();
                    break;
                case SearchProgressReport.SortEnum.BenchAsc:
                    List = List.OrderBy(p => p.Bench).ToList();
                    break;
                case SearchProgressReport.SortEnum.BenchDesc:
                    List = List.OrderByDescending(p => p.Bench).ToList();
                    break;
                case SearchProgressReport.SortEnum.CleanAsc:
                    List = List.OrderBy(p => p.Clean).ToList();
                    break;
                case SearchProgressReport.SortEnum.CleanDesc:
                    List = List.OrderByDescending(p => p.Clean).ToList();
                    break;
                default:
                    List = List.OrderByDescending(p => p.Total).ToList();
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

        protected int GetRankValue(Record record, SearchProgressReport.SortEnum sort)
        {
            return record.Total;
            //switch (Search.Sort)
            //{
            //    case SearchProgressReport.SortEnum.TotalAsc:
            //    case SearchProgressReport.SortEnum.TotalDesc:
            //        return record.Total;
            //    case SearchProgressReport.SortEnum.SquatAsc:
            //    case SearchProgressReport.SortEnum.SquatDesc:
            //        return record.Squat;
            //    case SearchProgressReport.SortEnum.BenchAsc:
            //    case SearchProgressReport.SortEnum.BenchDesc:
            //        return record.Bench;
            //    case SearchProgressReport.SortEnum.CleanAsc:
            //    case SearchProgressReport.SortEnum.CleanDesc:
            //        return record.Clean;
            //    default:
            //        return record.Total;
            //}
        }

        protected virtual void GetPage()
        {
            List = List.Skip((Search.Page - 1) * 20).Take(20).ToList();
        }
    }
}