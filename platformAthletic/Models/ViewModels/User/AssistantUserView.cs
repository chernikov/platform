using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public class AssistantUserView : BaseUserView
    {
     
        public int AssistantOfTeamID { get; set; }

        public string AvatarPath { get; set; }

        public int? GroupID { get; set; }

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

        [Required(ErrorMessage="Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Password {get; set;}
    }
}