using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using StackExchange.Profiling;

namespace platformAthletic.Models.Info
{
    public class NationalLeaderboard
    {
        public class Record
        {
            public int Position { get; set; }

            public User User { get; set; }

            public int Squat { get; set; }

            public int Bench { get; set; }

            public int Clean { get; set; }

            public int Total { get; set; }
        }

        protected IRepository Repository = DependencyResolver.Current.GetService<IRepository>();

        public List<Record> List { get; set; }

        public int TotalCount { get; set; }

        public int CountPage
        {
            get
            {
                return (int)decimal.Remainder(TotalCount, SearchNationalLeaderboard.PageSize) == 0 ? TotalCount / SearchNationalLeaderboard.PageSize : TotalCount / SearchNationalLeaderboard.PageSize + 1;
            }
        }
        public SearchNationalLeaderboard Search { get; set; }
      

        public NationalLeaderboard(SearchNationalLeaderboard search)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Calc NationalLeaderboard"))
            {
                List = new List<Record>();
                Search = search;
                Process();
            }
        }

        protected virtual void Process()
        {
            var users = Repository.TeamPlayersUsers;
            SearchNationalLeaderboardFilter.CutSearchCriteria(Search, users, preFilter : true);
            Filter(ref users);
            var orderUsers= Order(users);
            FillRecords(orderUsers);
            SearchNationalLeaderboardFilter.CutSearchCriteria(Search, users);
        }

        protected virtual void FillRecords(IOrderedQueryable<User> orderUsers)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Fill Records"))
            {
                TotalCount = orderUsers.Count();
                var i = 1;
                var rank = 0;
                int rankValue = -1;
                int currentValue = 0;
                foreach (var user in orderUsers)
                {
                    currentValue = GetRankValue(user, Search.Sort);
                    if (currentValue != rankValue)
                    {
                        rank = i;
                        rankValue = currentValue;
                    }
                    List.Add(new Record()
                    {
                        Position = rank,
                        User = user,
                        Squat = (int)user.Squat,
                        Bench = (int)user.Bench,
                        Clean = (int)user.Clean,
                        Total = (int)(user.Squat + user.Bench + user.Clean)
                    });
                    i++;
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
                GetPage();
            }
        }

        protected virtual void GetPage()
        {
            List = List.Skip((Search.Page - 1) * 20).Take(20).ToList();
        }

        protected int GetRankValue(User user, SearchNationalLeaderboard.SortEnum sort)
        {
            switch (Search.Sort)
            {
                case SearchNationalLeaderboard.SortEnum.NameAsc:
                case SearchNationalLeaderboard.SortEnum.NameDesc:
                    return (user.FirstName + user.LastName).GetHashCode();
                case SearchNationalLeaderboard.SortEnum.SquatAsc:
                case SearchNationalLeaderboard.SortEnum.SquatDesc:
                    return (int)user.Squat;
                case SearchNationalLeaderboard.SortEnum.BenchAsc:
                case SearchNationalLeaderboard.SortEnum.BenchDesc:
                    return (int)user.Bench;
                case SearchNationalLeaderboard.SortEnum.CleanAsc:
                case SearchNationalLeaderboard.SortEnum.CleanDesc:
                    return (int)user.Clean;
                default:
                    return (int)(user.Squat + user.Clean + user.Bench);
            }
        }

        protected IOrderedQueryable<User> Order(IQueryable<User> users)
        {
            switch (Search.Sort)
            {
                case SearchNationalLeaderboard.SortEnum.NameAsc:
                    return users.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                case SearchNationalLeaderboard.SortEnum.NameDesc:
                    return users.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                case SearchNationalLeaderboard.SortEnum.SquatAsc:
                    return users.OrderBy(p => p.Squat);
                case SearchNationalLeaderboard.SortEnum.SquatDesc:
                    return users.OrderByDescending(p => p.Squat);
                case SearchNationalLeaderboard.SortEnum.BenchAsc:
                    return users.OrderBy(p => p.Bench);
                case SearchNationalLeaderboard.SortEnum.BenchDesc:
                    return users.OrderByDescending(p => p.Bench);
                case SearchNationalLeaderboard.SortEnum.CleanAsc:
                    return users.OrderBy(p => p.Clean);
                case SearchNationalLeaderboard.SortEnum.CleanDesc:
                    return users.OrderByDescending(p => p.Clean);
                case SearchNationalLeaderboard.SortEnum.TotalAsc:
                    return users.OrderBy(p => p.Squat + p.Clean + p.Bench);
                case SearchNationalLeaderboard.SortEnum.TotalDesc:
                    return users.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                default:
                    return users.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
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
            if (Search.Gender != null)
            {
                users = users.Where(p => p.Gender == Search.Gender.Value);
            }
            if (Search.StateID != null)
            {
                //TeamOfPlay
                users = users.Where(p => p.Team.StateID == Search.StateID.Value);
            }
            if (Search.LevelID != null)
            {
                users = users.Where(p => p.LevelID == Search.LevelID.Value);
                if (Search.GradYear != null)
                {
                    users = users.Where(p => p.GradYear == Search.GradYear.Value);
                }
            }
            if (Search.Age.HasValue)
            {
                users = users.Where(p => p.Birthday != null && (p.Birthday > Search.MinDate && p.Birthday < Search.MaxDate));
            }
        }

       
    }
};