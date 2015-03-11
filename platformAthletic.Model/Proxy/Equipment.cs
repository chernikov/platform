using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Equipment
    {
        public string FullImagePath
        {
            get
            {
                return string.IsNullOrWhiteSpace(ImagePath) ? "/Media/files/equipment.png" : ImagePath;
            }
        }
	}
}