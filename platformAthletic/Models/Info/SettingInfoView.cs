using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    
    public class SettingInfoView
    {

        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string  PhoneNumber { get; set; }

        public int SBCControl { get; set; }

        public int SBCAttendance { get; set; }

        public int AttendanceControl { get; set; }

        public int PublicLevel { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public string LogoPath { get; set; }

    }
}