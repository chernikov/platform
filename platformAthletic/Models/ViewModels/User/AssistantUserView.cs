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

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password {get; set;}
    }
}