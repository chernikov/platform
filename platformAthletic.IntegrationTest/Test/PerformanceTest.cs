using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading;
using platformAthletic.Models.Info;
using platformAthletic.Model;
using System.Web.Mvc;

namespace platformAthletic.IntegrationTest.Test
{
    [TestFixture]
    public class PerformanceTest
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        [Test]
        public void Performance_TotalProgressReport() 
        {
            var Repository = DependencyResolver.Current.GetService<IRepository>();
            var teams = Repository.Teams.ToList();
            _stopwatch.Start();
            foreach(var team in teams) 
            {
                var filterCustomProgressReport = new FilterCustomProgressReport()
                {
                    BeginDate = new DateTime(2013, 1, 1),
                    EndDate = DateTime.Now,
                    TeamID = team.ID, 
                    SelectedType = FilterCustomProgressReport.Type.Team, 
                    Weight = FilterCustomProgressReport.WeightEnum.All,
                    SelectedPeriod = FilterCustomProgressReport.Period.ByWeek
                };

                var output = new Dictionary<DateTime, double>();

                var currentDate = filterCustomProgressReport.BeginDate;
                var endDate = filterCustomProgressReport.EndDate;
                while (currentDate < endDate)
                {
                    var value = GetUserWeightData(team, filterCustomProgressReport, currentDate);
                    output.Add(currentDate, double.IsNaN(value) ? 0 : value);
                    switch (filterCustomProgressReport.SelectedPeriod)
                    {
                        case FilterCustomProgressReport.Period.ByWeek:
                            currentDate = currentDate.AddDays(7);
                            break;
                        case FilterCustomProgressReport.Period.ByMonth:
                            currentDate = currentDate.AddMonths(1);
                            break;
                    }
                }
                if (!output.Any())
                {
                    output.Add(DateTime.Now, 0);
                }
                var data = output.Select(p => new
                {
                    p.Key,
                    Value = Math.Round(p.Value, 0, MidpointRounding.ToEven)

                }).ToArray();
                _stopwatch.Stop();
                Console.WriteLine("Team {0} Time {1}", team.Name, _stopwatch.Elapsed);
            }
        }

        private double GetUserWeightData(Team team, FilterCustomProgressReport filter, DateTime beginDate)
        {
            double total = 0;
            int count = 0;

            switch (filter.SelectedType)
            {
                case FilterCustomProgressReport.Type.Team:
                    foreach (var user in team.Users.ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Group:
                    foreach (var user in team.Users.Where(p => p.GroupID == filter.GroupID).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Individual:
                    foreach (var user in team.Users.Where(p => p.ID == filter.UserID).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
                case FilterCustomProgressReport.Type.Position:
                    foreach (var user in team.Users.Where(p => p.FieldPositionID == filter.FieldPositionID).ToList())
                    {
                        var value = GetUserValue(user, filter.Weight, beginDate);
                        if (value.HasValue)
                        {
                            total += value.Value;
                            count++;
                        }
                    }
                    break;
            }
            return total / count;
        }

        private double? GetUserValue(User user, FilterCustomProgressReport.WeightEnum weight, DateTime beginDate)
        {
            var record = user.SBCValues.Where(p => p.AddedDate <= beginDate).OrderByDescending(p => p.ID).FirstOrDefault();
            if (record != null)
            {
                switch (weight)
                {
                    case FilterCustomProgressReport.WeightEnum.All:
                        return record.Clean + record.Squat + record.Bench;
                    case FilterCustomProgressReport.WeightEnum.Squat:
                        return record.Squat;
                    case FilterCustomProgressReport.WeightEnum.Bench:
                        return record.Bench;
                    case FilterCustomProgressReport.WeightEnum.Clean:
                        return record.Clean;
                }
            }
            return null;
        }
    }
}
