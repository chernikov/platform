using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Setting
    {
        public int ValueInt
        {
            get
            {
                int intVal = 0;
                if (int.TryParse(Value, out intVal))
                {
                    return intVal;
                }
                return 0;
            }
        }

        public double ValueDouble
        {
            get
            {
                double doubleVal = 0;
                if (double.TryParse(Value, out doubleVal))
                {
                    return doubleVal;
                }
                return 0;
            }
        }
	}
}