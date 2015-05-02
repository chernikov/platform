using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class AttendanceInfo
    {
        public User User { get; set; }

        public DateTime Date { get; set; }

        public List<DateTime> Attendances { get; set; }

        public DateTime StartMonthSunday
        {
            get
            {
                var firstDay = new DateTime(Date.Year, Date.Month, 1);
                int diff = firstDay.DayOfWeek - DayOfWeek.Sunday;
                if (diff < 0)
                {
                    diff += 7;
                }
                return firstDay.AddDays(-1 * diff).Date;
            }
        }

        public DateTime LastMonthDate
        {
            get
            {
                return new DateTime(Date.Year, Date.Month+1, -1);
            }
        }
    }
}