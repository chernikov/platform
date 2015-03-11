using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class PromoCode
    {
        public bool Used
        {
            get
            {
                return UsedDate.HasValue;
            }
        }
	}
}