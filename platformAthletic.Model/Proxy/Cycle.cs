using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Cycle
    {
        public IEnumerable<Macrocycle> Macrocycles
        {
            get
            {
                return Phases.SelectMany(p => p.Weeks).SelectMany(p => p.Macrocycles);
            }
        }

        public int FirstNumberOfWeek
        {
            get
            {
                var week = Phases.SelectMany(p => p.Weeks).Where(p => p.Number.HasValue).OrderBy(p => p.Number).FirstOrDefault();
                return week != null ? (week.Number ?? 0) : 0;
            }
        }
	}
}