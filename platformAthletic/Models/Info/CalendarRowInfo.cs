using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.ViewModels
{
    public class CalendarRowInfo
    {
        public DateTime CurrentSunday { get; set; }

        public Macrocycle Macrocycle { get; set; }

        public int NumberOfWeek { get; set; }

        public bool IsDefault { get; set; }

    }
}