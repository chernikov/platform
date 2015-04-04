using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public class RegisterIndividualView : RegisterUserView
    {
        [Required(ErrorMessage="Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }


        [ValidPhone(ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }

        public BillingInfoView BillingInfo { get; set; }

        public RegisterIndividualView()
        {
            BillingInfo = new BillingInfoView();
        }

        public override string ReferralCode
        {
            get
            {
                if (BillingInfo != null)
                {
                    return BillingInfo.ReferralCode;
                }
                return string.Empty;
            }
            set
            {
                if (BillingInfo != null)
                {
                    BillingInfo.ReferralCode = value;
                }
            }
        }

        public override double TotalSum
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                var totalSum = repository.Settings.First(p => p.Name == "IndividualPrice").ValueDouble;

                if (!PromoCodeID.HasValue)
                {
                    return totalSum;
                }
                else
                {
                    var discountSum = repository.GetDiscountByPromoCode(PromoCodeID.Value, totalSum, PromoAction.TargetEnum.IndividualSubscription);
                    return discountSum;
                }
            }
        }

        public override PromoAction.TargetEnum Target
        {
            get 
            {
                return PromoAction.TargetEnum.IndividualSubscription;
            }
        }
    }
}