using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Macrocycle
    {
        public Season Season
        {
            get
            {
                return Week.Phase.Cycle.Season;
            }
        }
	}
}