using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class Week
    {
        public string FullName
        {
            get
            {
                return Name + " " + Phase.Name + " " + Phase.Cycle.Name + " " + Phase.Cycle.Season.Name;
            }
        }

        public string MacrocycleName
        {
            get
            {
                var macrocycle = Macrocycles.FirstOrDefault();
                if (macrocycle != null)
                {
                    return macrocycle.Name;
                }
                else
                {
                    return FullName;
                }

            }
        }

        public Macrocycle Macrocycle
        {
            get
            {
                return Macrocycles.FirstOrDefault();
            }
        }
    }
}