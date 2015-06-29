using platformAthletic.Attributes.Validation;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{

    public class SettingInfoView : BaseUserView
    {
        [Required(ErrorMessage = "Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter phone")]
        [ValidPhone(ErrorMessage = "Enter valid phone")]
        public string PhoneNumber { get; set; }

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

        public string FullLogoPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LogoPath))
                {
                    return "/Media/images/no-userpic.png";
                }
                return LogoPath;
            }
        }

        public string AvatarPath { get; set; }

        public string FullAvatarPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarPath))
                {
                    return "/Media/images/no-userpic.png";
                }
                return AvatarPath;
            }
        }
    }
}