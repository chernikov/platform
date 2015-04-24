using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class SchoolRankInfo
    {
        private IRepository Repository = DependencyResolver.Current.GetService<IRepository>();

        public class Rank
        {
            public int Squat { get; set; }

            public int Bench { get; set; }

            public int Clean { get; set; }

            public int Total { get; set; }
        }

        public Rank School { get; set; }

        public SchoolRankInfo(User user)
        {
            var schoolUsers = Repository.TeamPlayersUsers.Where(p => p.PlayerOfTeamID == user.PlayerOfTeamID);
            School = new Rank()
            {
                Squat = schoolUsers.Count(p => p.Squat > user.Squat) + 1,
                Bench = schoolUsers.Count(p => p.Bench > user.Bench) + 1,
                Clean = schoolUsers.Count(p => p.Clean > user.Clean) + 1,
                Total = schoolUsers.Count(p => p.Squat + p.Bench + p.Clean > user.Total) + 1
            };
        }
    }
}