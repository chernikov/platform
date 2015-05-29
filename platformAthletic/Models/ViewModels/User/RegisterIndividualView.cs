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


        public int IndividualStateID { get; set; }

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