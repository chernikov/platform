using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class ScheduleTableInfo
    {
        public List<ScheduleTableItem> List { get; set; }

        public DateTime StartDate { get; set; }

        public int Count { get; set; }

        public ScheduleTableInfo(IRepository repository, DateTime startDate, int count, int userID)
        {
            StartDate = startDate;
            Count = count;
            List = new List<ScheduleTableItem>();
            foreach (var team in repository.Teams.Where(p => p.UserID == userID))
            {
                var item = new ScheduleTableItem() 
                {
                    Team = team
                };
                item.Init(repository, startDate, count);
                List.Add(item);
                foreach (var group in team.Groups)
                {
                    var subItem = new ScheduleTableItem()
                    {
                        Team = team, Group = group
                    };
                    subItem.Init(repository, startDate, count);

                    List.Add(subItem); 
                }
            }
        }
        
    }
}