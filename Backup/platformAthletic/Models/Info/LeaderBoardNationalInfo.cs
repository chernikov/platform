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
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public double? Value { get; set; }

            public string InitialAndLastName
            {
                get
                {
                    return (FirstName.Substring(0, 1) + ". " + LastName).ToUpper();
                }
            }
        }

        public class TopRecords
        {
            public List<Record> Records { get; set; }


            public Record this[int index]
            {
                get
                {
                    if (Records.Count > index)
                    {
                        return Records[index];
                    }
                    else
                    {
                        return new Record();
                    }
                }
            }

            public TopRecords()
            {
                Records = new List<Record>(10);
            }
        }

        public Dictionary<FieldPosition, TopRecords> List { get; set; }

        public Record AllTimeSquat { get; set; }

        public Record AllTimeBench { get; set; }

        public Record AllTimeClean { get; set; }

        public Record AllTimeAll { get; set; }


        public List<FieldPosition> FieldPositions { get; set; }

        public LeaderBoardNationalInfo(SearchNationalLeaderBoard search)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();

            List = new Dictionary<FieldPosition, TopRecords>();
            AllTimeSquat = new Record();
            AllTimeBench = new Record();
            AllTimeClean = new Record();
            AllTimeAll = new Record();
        
            var users = repository.Users.Where(p => p.PlayerOfTeamID.HasValue);

            if (search.StateID > 0) 
            {
                users = users.Where(p => p.Team.StateID == search.StateID);
            }
            //Except Athlete
            FieldPositions = repository.FieldPositions.Where(p => p.ID < 9).ToList();
            foreach (var fieldPosition in FieldPositions)
            {
                var topRecord = new TopRecords();

                var fieldValues = users.Where(p => p.FieldPositions.Any(r => r.ID == fieldPosition.ID));

                switch (search.TrainingType)
                {
                    case 0 : //all
                        topRecord.Records = fieldValues.OrderByDescending(p => p.Squat + p.Bench + p.Clean).Take(10).Select(p => new Record()
                        {
                            LastName = p.LastName,
                            FirstName = p.FirstName,
                            Value = p.Squat + p.Bench + p.Clean
                        }).ToList();
                        break;
                    case 1: //squat
                        topRecord.Records = fieldValues.OrderByDescending(p => p.Squat).Take(10).Select(p => new Record()
                        {
                            LastName = p.LastName,
                            FirstName = p.FirstName,
                            Value = p.Squat 
                        }).ToList();
                        break;
                    case 2: //bench
                        topRecord.Records = fieldValues.OrderByDescending(p => p.Bench).Take(10).Select(p => new Record()
                        {
                            LastName = p.LastName,
                            FirstName = p.FirstName,
                            Value = p.Bench
                        }).ToList();
                        break;
                    case 3: //clean
                        topRecord.Records = fieldValues.OrderByDescending(p => p.Clean).Take(10).Select(p => new Record()
                        {
                            LastName = p.LastName,
                            FirstName = p.FirstName,
                            Value = p.Clean
                        }).ToList();
                        break;
                }
                List.Add(fieldPosition, topRecord);
            }

            var squatUser = repository.SBCValues.OrderByDescending(p => p.Squat).FirstOrDefault();
            if (squatUser != null)
            {
                AllTimeSquat.LastName = squatUser.LastName;
                AllTimeSquat.FirstName = squatUser.FirstName;
                AllTimeSquat.Value = squatUser.Squat;
            }
            var benchUser = repository.SBCValues.OrderByDescending(p => p.Bench).FirstOrDefault();
            if (benchUser != null)
            {
                AllTimeBench.LastName = benchUser.LastName;
                AllTimeBench.FirstName = benchUser.FirstName;
                AllTimeBench.Value = benchUser.Squat;
            }
            var cleanUser = repository.SBCValues.OrderByDescending(p => p.Clean).FirstOrDefault();
            if (cleanUser != null)
            {
                AllTimeClean.LastName = cleanUser.LastName;
                AllTimeClean.FirstName = cleanUser.FirstName;
                AllTimeClean.Value = cleanUser.Clean;
            }

            var allTimeAllUser = repository.SBCValues.OrderByDescending(p => p.Squat + p.Bench + p.Clean).FirstOrDefault();
            if (allTimeAllUser != null)
            {
                AllTimeAll.LastName = allTimeAllUser.LastName;
                AllTimeAll.FirstName = allTimeAllUser.FirstName;
                AllTimeAll.Value = allTimeAllUser.Squat + allTimeAllUser.Bench + allTimeAllUser.Clean;
            }
        }
    }
}