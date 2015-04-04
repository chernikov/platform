using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using platformAthletic.Attributes.Validation;

namespace platformAthletic.Models.ViewModels.User
{
    public class TeamUserView : BaseUserView
    {
        [Required(ErrorMessage = "Enter Full Name")]
        public string FullName { get; set; }

        public TeamView Team { get; set; }

        [ValidPhone(ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }
    }
}