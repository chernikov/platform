using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class LeaderBoardNationalInfo
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

        public List<Record> TopList { get; set; }

        public List<Record> List { get; set; }

        public int TotalCount { get; set; }

        public int CountPage
        {
            get
            {
                return (int)decimal.Remainder(TotalCount, SearchNationalLeaderBoard.PageSize) == 0 ? TotalCount / SearchNationalLeaderBoard.PageSize : TotalCount / SearchNationalLeaderBoard.PageSize + 1;
            }
        }

        public SearchNationalLeaderBoard Search { get; set; }

        public LeaderBoardNationalInfo(SearchNationalLeaderBoard search, bool allPages = false)
        {
            Search = search;

            TopList = new List<Record>();
            List = new List<Record>();

            IQueryable<SBCValue> sbcValues;
            IQueryable<User> users;
            IOrderedQueryable<SBCValue> topOrderSbcValues;
            IOrderedQueryable<User> orderUsers;

            var repository = DependencyResolver.Current.GetService<IRepository>();
            sbcValues = repository.SBCValues;
            users = repository.Users;
            CutSearchCriteria(users, true);
            Filter(ref sbcValues,ref users);
            if (allPages)
            {
                Order(sbcValues, users, out topOrderSbcValues, out orderUsers);
                MakeUnpageChart(orderUsers);
            }
            else
            {
                Order(sbcValues, users, out topOrderSbcValues, out orderUsers);
                MakeTopAllTime(topOrderSbcValues);
                MakeChart(orderUsers);
                CutSearchCriteria(users);
            }
        }

        private void MakeUnpageChart(IOrderedQueryable<User> orderUsers)
        {
            TotalCount = orderUsers.Count();
            var i = 1;
            foreach (var user in orderUsers)
            {
                List.Add(new Record()
                {
                    Position = i,
                    User = user,
                    Squat = (int)user.Squat,
                    Bench = (int)user.Bench,
                    Clean = (int)user.Clean,
                    Total = (int)(user.Squat + user.Bench + user.Clean)
                });
                i++;
            }
        }

        private void MakeChart(IOrderedQueryable<User> orderUsers)
        {
            var localUsers = orderUsers.Skip((Search.Page - 1) * 20).Take(20);
            TotalCount = orderUsers.Count();

            var i = (Search.Page - 1) * 20 + 1;
            foreach (var user in localUsers)
            {
                List.Add(new Record()
                {
                    Position = i,
                    User = user,
                    Squat = (int)user.Squat,
                    Bench = (int)user.Bench,
                    Clean = (int)user.Clean,
                    Total = (int)(user.Squat + user.Bench + user.Clean)
                });
                i++;
            }
        }

        private void MakeTopAllTime(IOrderedQueryable<SBCValue> topOrderSbcValues)
        {
            var i = 1;
            foreach (var sbcValue in topOrderSbcValues)
            {
                if (!TopList.Any(p => p.User.ID == sbcValue.UserID))
                {
                    TopList.Add(new Record()
                    {
                        Position = i,
                        User = sbcValue.User,
                        Squat = (int)sbcValue.Squat,
                        Bench = (int)sbcValue.Bench,
                        Clean = (int)sbcValue.Clean,
                        Total = (int)(sbcValue.Squat + sbcValue.Bench + sbcValue.Clean)
                    });
                    if (i == 3)
                    {
                        break;
                    }
                    i++;
                }
            }
        }

        private void Order(IQueryable<SBCValue> sbcValues, IQueryable<User> users, out IOrderedQueryable<SBCValue> topOrderSbcValues, out IOrderedQueryable<User> orderUsers)
        {
            switch (Search.Sort)
            {
                case SearchNationalLeaderBoard.SortEnum.NameAsc:
                    orderUsers = users.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
                    topOrderSbcValues = sbcValues.OrderBy(p => p.User.LastName).ThenBy(p => p.User.FirstName);
                    break;
                case SearchNationalLeaderBoard.SortEnum.NameDesc:
                    orderUsers = users.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.User.LastName).ThenByDescending(p => p.User.FirstName);
                    break;
                case SearchNationalLeaderBoard.SortEnum.SquatAsc:
                    orderUsers = users.OrderBy(p => p.Squat);
                    topOrderSbcValues = sbcValues.OrderBy(p => p.Squat);
                    break;
                case SearchNationalLeaderBoard.SortEnum.SquatDesc:
                    orderUsers = users.OrderByDescending(p => p.Squat);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.Squat);
                    break;
                case SearchNationalLeaderBoard.SortEnum.BenchAsc:
                    orderUsers = users.OrderBy(p => p.Bench);
                    topOrderSbcValues = sbcValues.OrderBy(p => p.Bench);
                    break;
                case SearchNationalLeaderBoard.SortEnum.BenchDesc:
                    orderUsers = users.OrderByDescending(p => p.Bench);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.Bench);
                    break;
                case SearchNationalLeaderBoard.SortEnum.CleanAsc:
                    orderUsers = users.OrderBy(p => p.Clean);
                    topOrderSbcValues = sbcValues.OrderBy(p => p.Clean);
                    break;
                case SearchNationalLeaderBoard.SortEnum.CleanDesc:
                    orderUsers = users.OrderByDescending(p => p.Clean);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.Clean);
                    break;
                case SearchNationalLeaderBoard.SortEnum.TotalAsc:
                    orderUsers = users.OrderBy(p => p.Squat + p.Clean + p.Bench);
                    topOrderSbcValues = sbcValues.OrderBy(p => p.Squat + p.Clean + p.Bench);
                    break;
                case SearchNationalLeaderBoard.SortEnum.TotalDesc:
                    orderUsers = users.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                    break;
                default : 
                    orderUsers = users.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                    topOrderSbcValues = sbcValues.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                    break;
            }
        }

        private void Filter(ref IQueryable<SBCValue> sbcValues,ref IQueryable<User> users)
        {
            if (Search.SportID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.UserFieldPositions.Any(r => r.SportID == Search.SportID));
                users = users.Where(p => p.UserFieldPositions.Any(r => r.SportID == Search.SportID));
            }
            if (Search.FieldPositionID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.UserFieldPositions.Any(r => r.FieldPositionID == Search.FieldPositionID));
                users = users.Where(p => p.UserFieldPositions.Any(r => r.FieldPositionID == Search.FieldPositionID));
            }
            if (Search.Gender != null)
            {
                sbcValues = sbcValues.Where(p => p.User.Gender == Search.Gender.Value);
                users = users.Where(p => p.Gender == Search.Gender.Value);
            }
            if (Search.StateID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.Team.StateID == Search.StateID.Value);
                users = users.Where(p => p.Team.StateID == Search.StateID.Value);
            }
            if (Search.LevelID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.LevelID == Search.LevelID.Value);
                users = users.Where(p => p.LevelID == Search.LevelID.Value);

                if (Search.GradYear != null)
                {
                    sbcValues = sbcValues.Where(p => p.User.GradYear == Search.GradYear.Value);
                    users = users.Where(p => p.GradYear == Search.GradYear.Value);
                }
            }

            if (Search.Age != null)
            {
                DateTime start;
                DateTime end;
                GetStartEndDateByAge(Search.Age.Value, out start, out end);
                sbcValues = sbcValues.Where(p => p.User.Birthday != null && (p.User.Birthday > start && p.User.Birthday < end));
                users = users.Where(p => p.Birthday != null && (p.Birthday > start && p.Birthday < end));
            }
        }

        private void GetStartEndDateByAge(int age, out DateTime start, out DateTime end)
        {
            start = DateTime.Now.AddYears(-age);
            end = DateTime.Now.AddYears(-age + 5);
            if (age == 70)
            {
                start = DateTime.Now.AddYears(200);
            }
            if (age == 0)
            {
                start = DateTime.Now.AddYears(-15);
                end = DateTime.Now;
            }
        }

        private void CutSearchCriteria(IQueryable<User> users, bool force = false) 
        {
            if (!Search.SportID.HasValue || force)
            {
                //Sport
                foreach (var sport in Search.Sports.ToList())
                {
                    if (!users.Any(p => p.UserFieldPositions.Any(r => r.SportID == sport.ID)))
                    {
                        Search.Sports.Remove(sport);
                    }
                }
            }
            if (!Search.FieldPositionID.HasValue || force)
            {
                //FieldPosition
                foreach (var fieldPosition in Search.FieldPositions.ToList())
                {
                    if (!users.Any(p => p.UserFieldPositions.Any(r => r.FieldPositionID == fieldPosition.ID)))
                    {
                        Search.FieldPositions.Remove(fieldPosition);
                    }
                }
            }
            if (!Search.Age.HasValue || force)
            {
                //Age
                foreach (var age in Search.Ages.ToList())
                {
                    DateTime start;
                    DateTime end;
                    GetStartEndDateByAge(age, out start, out end);
                    if (!users.Any(p => p.Birthday != null && (p.Birthday > start && p.Birthday < end)))
                    {
                        Search.Ages.Remove(age);
                    }
                }
            }
            if (!Search.LevelID.HasValue || force)
            {
                //Levels
                foreach (var level in Search.Levels.ToList())
                {
                    if (!users.Any(p => p.LevelID == level.ID))
                    {
                        Search.Levels.Remove(level);
                    }
                }
            }

            //Grad Years
            if (Search.ShowGradYear)
            {
                Search.GradYears = users.Where(p => p.GradYear != null).Select(p => p.GradYear.Value).Distinct().ToList();
            }

            if (!Search.Gender.HasValue || force)
            {
                //Genders
                foreach (var gender in Search.Genders.ToList())
                {
                    if (!users.Any(p => p.Gender == gender))
                    {
                        Search.Genders.Remove(gender);
                    }
                }
            }
            if (!Search.StateID.HasValue || force)
            {
                //States
                foreach (var state in Search.States.ToList())
                {
                    if (!users.Any(p => p.Team.StateID == state.ID))
                    {
                        Search.States.Remove(state);
                    }
                }
            }
        }
    }
}