using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class RankInfo
    {
        private IRepository Repository = DependencyResolver.Current.GetService<IRepository>();

        public class Rank
        {
            public int Squat { get; set; }

            public int Bench { get; set; }

            public int Clean { get; set; }

            public int Total { get; set; }
        }

        public Rank Nation { get; set; }

        public Rank State { get; set; }

        public Rank School { get; set; }

        public RankInfo(User user)
        {
            var users = Repository.PlayersTeamPlayersUsers;
            //TeamOfPlay
            Nation = new Rank();
            State = new Rank();
            School = new Rank();
            var userStateID = user.IndividualStateID ?? (user.Team != null ? user.Team.StateID : 0);
            if (userStateID == 0)
            {
                return;
            }
            var stateUsers =  Repository.PlayersTeamPlayersUsers.Where(p => 
                (p.PlayerOfTeamID != null && p.Team.StateID == userStateID) || p.IndividualStateID == userStateID);
            var schoolUsers = Repository.TeamPlayersUsers.Where(p => p.PlayerOfTeamID == user.PlayerOfTeamID);
            try
            {
                Nation = new Rank()
                {
                    Squat = users.Count(p => p.Squat > user.Squat) + 1,
                    Bench = users.Count(p => p.Bench > user.Bench) + 1,
                    Clean = users.Count(p => p.Clean > user.Clean) + 1,
                    Total = users.Count(p => p.Squat + p.Bench + p.Clean > user.Total) + 1
                };

                State = new Rank()
                {
                    Squat = stateUsers.Count(p => p.Squat > user.Squat) + 1,
                    Bench = stateUsers.Count(p => p.Bench > user.Bench) + 1,
                    Clean = stateUsers.Count(p => p.Clean > user.Clean) + 1,
                    Total = stateUsers.Count(p => p.Squat + p.Bench + p.Clean > user.Total) + 1
                };

                School = new Rank()
                {
                    Squat = schoolUsers.Count(p => p.Squat > user.Squat) + 1,
                    Bench = schoolUsers.Count(p => p.Bench > user.Bench) + 1,
                    Clean = schoolUsers.Count(p => p.Clean > user.Clean) + 1,
                    Total = schoolUsers.Count(p => p.Squat + p.Bench + p.Clean > user.Total) + 1
                };
            }
            catch (Exception ex)
            {

            }
        }
    }
}