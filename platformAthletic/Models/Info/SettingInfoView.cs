using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    
    public class SettingInfoView
    {

        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string  PhoneNumber { get; set; }

        public int? IndividualStateID { get; set; }

        private IEnumerable<State> States
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.States.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListStateID
        {
            get
            {
                return States.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name,
                    Selected = p.ID == IndividualStateID
                });
            }
        }

        public int SBCControl { get; set; }

        public int SBCAttendance { get; set; }

        public int AttendanceControl { get; set; }

        public int PublicLevel { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public string LogoPath { get; set; }

    }
}