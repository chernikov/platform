using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class ProgressInfo
    {
        public User User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        private SBCValue StartValue { get; set; }


        private SBCValue EndValue { get; set; }

        public int Squat { get; private set; }


        public int Bench { get; private set; }


        public int Clean { get; private set; }

        public int Total
        {
            get
            {
                return Squat + Bench + Clean;
            }
        }

        public ProgressInfo(User user, DateTime startDate, DateTime endDate)
        {
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            StartValue = User.SBCHistory(StartDate);
            EndValue = User.SBCHistory(EndDate);
            if (EndValue != null && StartValue != null)
            {
                Squat = (int)(EndValue.Squat - StartValue.Squat);
                Bench = (int)(EndValue.Bench - StartValue.Bench);
                Clean = (int)(EndValue.Clean - StartValue.Clean);
            }
        }
    }
}