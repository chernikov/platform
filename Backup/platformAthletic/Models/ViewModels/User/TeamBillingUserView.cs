using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public class TeamBillingUserView : BillingUserView
    {
        public InvoiceView Invoice { get; set; }

        public TeamBillingUserView() : base()
        {
            Invoice = new InvoiceView();
        }

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