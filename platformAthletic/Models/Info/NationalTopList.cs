﻿using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class NationalTopList
    {

        private IRepository Repository = DependencyResolver.Current.GetService<IRepository>();


        public class Record
        {
            public int Position { get; set; }

            public User User { get; set; }

            public int Squat { get; set; }

            public int Bench { get; set; }

            public int Clean { get; set; }

            public int Total { get; set; }
        }

        public List<Record> TopAllTime { get; set; }

        public SearchNationalLeaderboard Search { get; set; }

        public NationalTopList(SearchNationalLeaderboard search)
        {
            Search = search;
            TopAllTime = new List<Record>();

            ProcessHistory();
        }

        public void ProcessHistory()
        {
            FillTopAllTime(OrderHistory(FilterHistory()));
        }
       
        protected IQueryable<SBCValue> FilterHistory()
        {
            var sbcValues = Repository.SBCValues;

            if (Search.SportID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.UserFieldPositions.Any(r => r.SportID == Search.SportID));
            }
            if (Search.FieldPositionID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.UserFieldPositions.Any(r => r.FieldPositionID == Search.FieldPositionID));
            }
            if (Search.Gender != null)
            {
                sbcValues = sbcValues.Where(p => p.User.Gender == Search.Gender.Value);
            }
            if (Search.StateID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.Team.StateID == Search.StateID.Value);
            }
            if (Search.LevelID != null)
            {
                sbcValues = sbcValues.Where(p => p.User.LevelID == Search.LevelID.Value);
                if (Search.GradYear != null)
                {
                    sbcValues = sbcValues.Where(p => p.User.GradYear == Search.GradYear.Value);
                }
            }
            if (Search.Age.HasValue)
            {
                sbcValues = sbcValues.Where(p => p.User.Birthday != null && (p.User.Birthday > Search.MinDate && p.User.Birthday < Search.MaxDate));
            }
            return sbcValues;
        }

        protected IOrderedQueryable<SBCValue> OrderHistory(IQueryable<SBCValue> sbcValues)
        {
            switch (Search.Sort)
            {
                case SearchNationalLeaderboard.SortEnum.NameAsc:
                    return sbcValues.OrderBy(p => p.User.LastName).ThenBy(p => p.User.FirstName);
                case SearchNationalLeaderboard.SortEnum.NameDesc:
                    return sbcValues.OrderByDescending(p => p.User.LastName).ThenByDescending(p => p.User.FirstName);
                case SearchNationalLeaderboard.SortEnum.SquatAsc:
                    return sbcValues.OrderBy(p => p.Squat);
                case SearchNationalLeaderboard.SortEnum.SquatDesc:
                    return sbcValues.OrderByDescending(p => p.Squat);
                case SearchNationalLeaderboard.SortEnum.BenchAsc:
                    return sbcValues.OrderBy(p => p.Bench);
                case SearchNationalLeaderboard.SortEnum.BenchDesc:
                    return sbcValues.OrderByDescending(p => p.Bench);
                case SearchNationalLeaderboard.SortEnum.CleanAsc:
                    return sbcValues.OrderBy(p => p.Clean);
                case SearchNationalLeaderboard.SortEnum.CleanDesc:
                    return sbcValues.OrderByDescending(p => p.Clean);
                case SearchNationalLeaderboard.SortEnum.TotalAsc:
                    return sbcValues.OrderBy(p => p.Squat + p.Clean + p.Bench);
                case SearchNationalLeaderboard.SortEnum.TotalDesc:
                    return sbcValues.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
                default:
                    return sbcValues.OrderByDescending(p => p.Squat + p.Clean + p.Bench);
            }
        }

        protected void FillTopAllTime(IOrderedQueryable<SBCValue> topOrderSbcValues)
        {
            var i = 1;
            foreach (var sbcValue in topOrderSbcValues)
            {
                if (!TopAllTime.Any(p => p.User.ID == sbcValue.UserID))
                {
                    TopAllTime.Add(new Record()
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

    }

}