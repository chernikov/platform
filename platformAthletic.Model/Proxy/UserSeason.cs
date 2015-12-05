using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class UserSeason
    {
        public int NumberOfWeek(DateTime date)
        {
            int numberOfWeek = (int)(((int)((date - this.StartDay).TotalDays) / 7));
            int totalWeeks = this.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
            return (numberOfWeek + (this.StartFrom > 0 ? this.StartFrom- 1 : 0)) % totalWeeks + 1;
        }
	}
}