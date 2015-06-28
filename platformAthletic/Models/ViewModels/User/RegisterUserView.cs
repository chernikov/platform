using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public abstract class RegisterUserView : BaseUserView
    {
        public enum RegisterTypeEnum : int
        {
            Coach = 0x01,
            Individual = 0x02
        }

        public enum PaymentTypeEnum : int 
        {
            CreditCard = 0x01,
            Invoice = 0x02
        }

        public int? PromoCodeID { get; set; }

        public RegisterTypeEnum RegisterType { get; set; }

        [Required(ErrorMessage = "Enter the password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

      

        public abstract string ReferralCode { get; set;  }

        public abstract double TotalSum { get; }

        public abstract PromoAction.TargetEnum Target { get; }
    }
}