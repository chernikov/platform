using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class LeaderBoardTeamInfo
    {
        public class Record
        {
            public FieldPosition FieldPosition { get; set; }

            public User CurrentUser { get; set; }

            public double? CurrentValue { get; set; }

            public string AllTimeLastName { get; set; }

            public string AllTimeFirstName { get; set; }

            public double? AllTimeValue { get; set; }

            public string AllTimeInitialAndLastName
            {
                get
                {
                    return (AllTimeFirstName.Substring(0, 1) + ". " + AllTimeLastName).ToUpper();
                }
            }
        }

        public List<Record> Squats { get; set; }

        public List<Record> Benchs { get; set; }

        public List<Record> Cleans { get; set; }

        public List<FieldPosition> FieldPositions { get; set; } 

        public Team Team { get; set; }

        public Record AllTimeAll { get; set; }

        public Record AllTimeSquat
        {
            get
            {
                return Squats.OrderByDescending(p => p.AllTimeValue).FirstOrDefault();
            }
        }

        public Record AllTimeBench
        {
            get
            {
                return Benchs.OrderByDescending(p => p.AllTimeValue).FirstOrDefault();
            }
        }

        public Record AllTimeClean
        {
            get
            {
                return Cleans.OrderByDescending(p => p.AllTimeValue).FirstOrDefault();
            }
        }

        public LeaderBoardTeamInfo(Team team)
        {
            Team = team;
            Squats = new List<Record>();
            Benchs = new List<Record>();
            Cleans = new List<Record>();

            
            var repository = DependencyResolver.Current.GetService<IRepository>();
            FieldPositions = repository.FieldPositions.ToList();
            foreach (var fieldPosition in FieldPositions)
            {
                var squatRecord = new Record();
                var maxSquatUser = team.Users.Where(p => p.FieldPositions.Any(r => r.ID == fieldPosition.ID)).OrderByDescending(p => p.Squat).FirstOrDefault();
                if (maxSquatUser != null)
                {
                    squatRecord.CurrentUser = maxSquatUser;
                    squatRecord.CurrentValue = maxSquatUser.Squat;
                }
                var maxSquatValue = team.SBCValues.Where(p => p.FieldPositionID == fieldPosition.ID).OrderByDescending(p => p.Squat).FirstOrDefault();
                if (maxSquatValue != null)
                {
                    squatRecord.AllTimeFirstName = maxSquatValue.FirstName;
                    squatRecord.AllTimeLastName = maxSquatValue.LastName;
                    squatRecord.AllTimeValue = maxSquatValue.Squat;
                }
                Squats.Add(squatRecord);

                var benchRecord = new Record();
                var maxBenchUser = team.Users.Where(p => p.FieldPositions.Any(r => r.ID == fieldPosition.ID)).OrderByDescending(p => p.Bench).FirstOrDefault();
                if (maxBenchUser != null)
                {
                    benchRecord.CurrentUser = maxBenchUser;
                    benchRecord.CurrentValue = maxBenchUser.Bench;
                }
                var maxBenchValue = team.SBCValues.Where(p => p.FieldPositionID == fieldPosition.ID).OrderByDescending(p => p.Bench).FirstOrDefault();
                if (maxBenchValue != null)
                {
                    benchRecord.AllTimeFirstName = maxBenchValue.FirstName;
                    benchRecord.AllTimeLastName = maxBenchValue.LastName;
                    benchRecord.AllTimeValue = maxBenchValue.Bench;
                }
                Benchs.Add(benchRecord);

                var cleanRecord = new Record();
                var maxCleanUser = team.Users.Where(p => p.FieldPositions.Any(r => r.ID == fieldPosition.ID)).OrderByDescending(p => p.Clean).FirstOrDefault();
                if (maxCleanUser != null)
                {
                    cleanRecord.CurrentUser = maxCleanUser;
                    cleanRecord.CurrentValue = maxCleanUser.Clean;
                }
                var maxCleanValue = team.SBCValues.Where(p => p.FieldPositionID == fieldPosition.ID).OrderByDescending(p => p.Clean).FirstOrDefault();
                if (maxCleanValue != null)
                {
                    cleanRecord.AllTimeFirstName = maxCleanValue.FirstName;
                    cleanRecord.AllTimeLastName = maxCleanValue.LastName;
                    cleanRecord.AllTimeValue = maxCleanValue.Clean;
                }
                Cleans.Add(cleanRecord);
            }

            AllTimeAll = new Record();
            var maxAllValue = team.SBCValues.OrderByDescending(p => p.Clean + p.Bench + p.Squat).FirstOrDefault();
            if (maxAllValue != null)
            {
                AllTimeAll.AllTimeFirstName = maxAllValue.FirstName;
                AllTimeAll.AllTimeLastName = maxAllValue.LastName;
                AllTimeAll.AllTimeValue = maxAllValue.Squat + maxAllValue.Bench + maxAllValue.Clean;
            }
        }
    }
}