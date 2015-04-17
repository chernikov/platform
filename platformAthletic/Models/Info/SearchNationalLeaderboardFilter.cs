using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class SearchNationalLeaderboardFilter
    {
        public static void CutSearchCriteria(SearchNationalLeaderboard Search, IQueryable<User> users, bool preFilter = false)
        {
            if (!Search.SportID.HasValue || preFilter)
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
            if (!Search.FieldPositionID.HasValue || preFilter)
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
            if (!Search.Age.HasValue || preFilter)
            {
                //Age
                foreach (var age in Search.Ages.ToList())
                {
                    DateTime maxDate;
                    DateTime minDate;
                    if (age == 70)
                    {
                        minDate = DateTime.Now.AddYears(200);
                        maxDate = DateTime.Now.AddYears(-age);
                    }
                    else if (age == 0)
                    {
                        minDate = DateTime.Now.AddYears(-15);
                        maxDate = DateTime.Now;
                    }
                    else
                    {
                        minDate = DateTime.Now.AddYears(-age);
                        maxDate = DateTime.Now.AddYears(-age + 5);
                    }
                    if (!users.Any(p => p.Birthday != null && (p.Birthday > minDate && p.Birthday < maxDate)))
                    {
                        Search.Ages.Remove(age);
                    }
                }
            }
            if (!Search.LevelID.HasValue || preFilter)
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

            if (!Search.Gender.HasValue || preFilter)
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
            if (!Search.StateID.HasValue || preFilter)
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