using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Cell
    {
        public enum CellType : int
        {
            Info = 0x01, 
            Exercise = 0x02, 
            Value = 0x03
        }
	}
}