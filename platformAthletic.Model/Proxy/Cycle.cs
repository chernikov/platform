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
	}
}