using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.ViewModels
{
    public class PersonalCalendarInfo
    {
        public Model.User User { get; set; }

        public DateTime Month { get; set; }

        public Season Season { get; set; }

        public IEnumerable<Season> Seasons { get; set; }
    }
}