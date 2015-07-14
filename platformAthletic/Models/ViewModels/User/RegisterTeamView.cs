using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;
using System.ComponentModel;

namespace platformAthletic.Models.ViewModels.User
{
    public class RegisterTeamView : RegisterUserView
    {
        public bool AgreementTermCondition { get; set; }

        public bool AgreementPrivacyPolicy { get; set; }

        public int PaymentType { get; set; }


        public TeamView Team { get; set; }

        public IEnumerable<SelectListItem> PaymentSelectList
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)PaymentTypeEnum.CreditCard).ToString(), Text = "Credit card", Selected = PaymentType == (int)PaymentTypeEnum.CreditCard };
                yield return new SelectListItem() { Value = ((int)PaymentTypeEnum.Invoice).ToString(), Text = "Invoice billing", Selected = PaymentType == (int)PaymentTypeEnum.Invoice };
            }
        }

        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [ValidPhone(ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }

        public RegisterTeamView()
        {
            Team = new TeamView();
           
        }

        public override string ReferralCode { get; set; }


        public override double TotalSum
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                var totalSum = repository.Settings.First(p => p.Name == "TeamPrice").ValueDouble;

                if (!PromoCodeID.HasValue)
                {
                    return totalSum;
                }
                else
                {
                    var discountSum = repository.GetDiscountByPromoCode(PromoCodeID.Value, totalSum, PromoAction.TargetEnum.TeamSubscription);
                    return discountSum;
                }
            }
        }

        public override PromoAction.TargetEnum Target
        {
            get
            {
                return PromoAction.TargetEnum.TeamSubscription;
            }
        }
    }
}