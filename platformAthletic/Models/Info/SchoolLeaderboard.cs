using platformAthletic.Model;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class SchoolLeaderboard
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

        public Team Team { get; set; }

        public List<Record> List { get; set; }

        public int TotalCount { get; set; }

        public int CountPage
        {
            get
            {
                return (int)decimal.Remainder(TotalCount, SearchNationalLeaderboard.PageSize) == 0 ? TotalCount / SearchNationalLeaderboard.PageSize : TotalCount / SearchNationalLeaderboard.PageSize + 1;
            }
        }
        public SearchSchoolLeaderboard Search { get; set; }

        public SchoolLeaderboard(SearchSchoolLeaderboard search, Team team)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Calc NationalLeaderboard"))
            {
                List = new List<Record>();
                Search = search;
                Team = team;
                Process();
            }
        }

        protected virtual void Process()
        {
            var users = Repository.TeamPlayersUsers.Where(p => p.PlayerOfTeamID == Team.ID);
            SearchSchoolLeaderboardFilter.CutSearchCriteria(Search, users, preFilter : true);
            Filter(ref users);
            var orderUsers= Order(users);
            FillRecords(orderUsers);
            SearchSchoolLeaderboardFilter.CutSearchCriteria(Search, users);
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

        protected int GetRankValue(User user, SearchSchoolLeaderboard.SortEnum sort)
        {
            switch (Search.Sort)
            {
                case SearchSchoolLeaderboard.SortEnum.NameAsc:
                case SearchSchoolLeaderboard.SortEnum.NameDesc:
                    return (user.FirstName + user.LastName).GetHashCode();
                case SearchSchoolLeaderboard.SortEnum.SquatAsc:
                case SearchSchoolLeaderboard.SortEnum.SquatDesc:
                    return (int)user.Squat;
                case SearchSchoolLeaderboard.SortEnum.BenchAsc:
                case SearchSchoolLeaderboard.SortEnum.BenchDesc:
                    return (int)user.Bench;
                case SearchSchoolLeaderboard.SortEnum.CleanAsc:
                case SearchSchoolLeaderboard.SortEnum.CleanDesc:
                    return (int)user.Clean;
                default:
                    return (int)(user.Squat + user.Clean + user.Bench);
            }
        }

        protected IOrderedQueryable<User> Order(IQueryable<User> users)
        {
            switch (Search.Sort)
            {
                case SearchSchoolLeaderboard.SortEnum.NameAsc:
                    return users.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                case SearchSchoolLeaderboard.SortEnum.NameDesc:
                    return users.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                case SearchSchoolLeaderboard.SortEnum.SquatAsc:
                    return users.OrderBy(p => p.Squat);
                case SearchSchoolLeaderboard.SortEnum.SquatDesc:
                    return users.OrderByDescending(p => p.Squat);
                case SearchSchoolLeaderboard.SortEnum.BenchAsc:
                    return users.OrderBy(p => p.Bench);
                case SearchSchoolLeaderboard.SortEnum.BenchDesc:
                    return users.OrderByDescending(p => p.Bench);
                case SearchSchoolLeaderboard.SortEnum.CleanAsc:
                    return users.OrderBy(p => p.Clean);
                case SearchSchoolLeaderboard.SortEnum.CleanDesc:
                    return users.OrderByDescending(p => p.Clean);
                case SearchSchoolLeaderboard.SortEnum.TotalAsc:
                    return users.OrderBy(p => p.Squat + p.Clean + p.Bench);
                case SearchSchoolLeaderboard.SortEnum.TotalDesc:
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
}