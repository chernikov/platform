using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class ScheduleTableItem
    {
        public Team Team { get; set; }

        public Group Group {get; set; }

        public List<ScheduleRowInfo> Schedules { get; set; }

        public ScheduleTableItem()
        {
            Schedules = new List<ScheduleRowInfo>();
        }

        public void Init(IRepository repository, DateTime startDate, int count)
        {
            var currentDate = startDate;
            for(int i = 0; i < count;i++) 
            {
                var row = AddRow(repository, currentDate);
                Schedules.Add(row);
                currentDate = currentDate.AddDays(7);
            }
        }

        private ScheduleRowInfo AddRow(IRepository repository, DateTime date)
        {
            var currentSeason = Team.User.SeasonByDateAndGroup(date, (int?)((Group != null) ? Group.ID : (int?)null), true);
            int numberOfWeek = currentSeason.NumberOfWeek(date); 
            if ((date - currentSeason.StartDay).TotalDays < 0)
            {
                numberOfWeek = -1;
            }
            var week = repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == currentSeason.SeasonID);
            if (week == null)
            {
                return new ScheduleRowInfo()
                {
                    CurrentSunday = date
                };
            }
            var macrocycle = week.Macrocycles.FirstOrDefault();
            var scheduleRowInfo = new ScheduleRowInfo()
            {
                UserSeason = currentSeason,
                CurrentSunday = date,
                Macrocycle = macrocycle,
                NumberOfWeek = numberOfWeek,
                IsDefault = true
            };
            if (Group != null)
            {
                var schedule = repository.Schedules.FirstOrDefault(p => p.Date == date && p.TeamID == Team.ID && p.GroupID == Group.ID && p.UserSeasonID == currentSeason.ID);
                if (schedule != null)
                {
                    scheduleRowInfo.Macrocycle = schedule.Macrocycle;
                    scheduleRowInfo.IsDefault = false;
                }
            }
            else
            {
                var schedule = repository.Schedules.FirstOrDefault(p => p.Date == date && p.TeamID == Team.ID && p.GroupID == null && p.UserSeasonID == currentSeason.ID);
                if (schedule != null)
                {
                    scheduleRowInfo.Macrocycle = schedule.Macrocycle;
                    scheduleRowInfo.IsDefault = false;
                }
            }
            return scheduleRowInfo;
            
            
        }
    }
}