using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public abstract class BaseUserView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Email")]
        [ValidEmail(ErrorMessage = "Enter correct Email")]
        [UserEmailValidation(ErrorMessage = "Email already registered")]
        public string Email { get; set; }

        public int CurrentSeasonSeasonType { get; set; }

        public DateTime CurrentSeasonStartDay { get; set; }

        public DateTime MinNextSelectDay
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<SqlRepository>();
                var currentDate = DateTime.Now;
                if (CurrentSeasonStartDay < currentDate)
                {
                    return currentDate;
                }
                return CurrentSeasonStartDay;
            }
        }
        
        public DateTime? NextSeasonStartDay { get; set; }
    }
}