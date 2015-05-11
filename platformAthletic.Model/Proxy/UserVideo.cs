using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class UserVideo
    {

        public string ThumbVideoCode
        {
            get 
            {
                return VideoCode.Replace("width='800'", "width='280'").Replace("height='600'", "height='210'");
            }
            
        }
    }
}
