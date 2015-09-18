using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class ProgressGraphInfo
    {
        public User User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        //public SBCValue StartSBC { get; set; }

        //public SBCValue EndSBC { get; set; }

        public int Squat { get; set; }

        public int Bench { get; set; }

        public int Clean { get; set; }
    }
}