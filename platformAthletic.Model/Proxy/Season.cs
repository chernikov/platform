using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Season
    {
        public enum TypeEnum
        {
            OffSeason = 0x00,
            InSeason = 0x01, 
        }

        public int DaysLength
        {
            get 
            {
                return Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count() * 7;
            }
        }

        public IEnumerable<Macrocycle> Macrocycles
        {
            get
            {
                return Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).SelectMany(p => p.Macrocycles);
            }
        }
	}
}