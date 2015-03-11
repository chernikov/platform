using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public abstract class BillingUserView
    {
        public int ID { get; set; }

        public BillingInfoView BillingInfo { get; set; }

        public DateTime PaidTill { get; set; }

        public int? PromoCodeID { get; set; }

        public abstract double TotalSum { get; }

        public string ReferralCode { get; set; }

        public BillingUserView()
        {
            BillingInfo = new BillingInfoView();
        }

        public abstract PromoAction.TargetEnum Target { get; }
    }
}