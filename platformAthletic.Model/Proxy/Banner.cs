using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Banner
    {
        public string Show
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Code))
                {
                    return Code;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Link))
                    {
                        return "<a href='"+Link+"' target='_blank'><img src='" + ImagePath + "' /></a>";
                    }
                    else
                    {
                        return "<img src='" + ImagePath + "' />";
                    }
                }
                
            }
        }
	}
}