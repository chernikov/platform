using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using platformAthletic.Attributes.Validation;

namespace platformAthletic.Models.ViewModels.User
{
    public class IndividualUserView : BaseUserView
    {
        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [ValidPhone(ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

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