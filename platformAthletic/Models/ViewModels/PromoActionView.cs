using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PromoActionView
    {
        public int ID { get; set; }

        [Required(ErrorMessage="Please enter Name")]
		public string Name {get; set; }

		public int Type {get; set; }
        
        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TypeEnum.Absolute).ToString(),
                    Text = "$",
                    Selected = Type == (int)PromoAction.TypeEnum.Absolute
                };
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TypeEnum.Percentage).ToString(),
                    Text = "%",
                    Selected = Type == (int)PromoAction.TypeEnum.Percentage
                };
            }
        }

        public int Target { get; set; }

        public IEnumerable<SelectListItem> SelectListTarget
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TargetEnum.Both).ToString(),
                    Text = "Both",
                    Selected = Type == (int)PromoAction.TargetEnum.Both
                };
                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TargetEnum.IndividualSubscription).ToString(),
                    Text = "Individual",
                    Selected = Type == (int)PromoAction.TargetEnum.IndividualSubscription
                };

                yield return new SelectListItem()
                {
                    Value = ((int)PromoAction.TargetEnum.TeamSubscription).ToString(),
                    Text = "Team",
                    Selected = Type == (int)PromoAction.TargetEnum.TeamSubscription
                };
            }
        }

		public double Amount {get; set; }

		public DateTime? ValidDate {get; set; }

        public int Quantity { get; set; }

        public bool Reusable { get; set; }

        public string ReferralCode { get; set; }

        public IEnumerable<SelectListItem> SelectListReusable
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "false",
                    Text = "No",
                    Selected = !Reusable
                };

                yield return new SelectListItem()
                {
                    Value = "true",
                    Text = "Yes",
                    Selected = Reusable
                };
            }
        }

        public bool CanChangeReusable { get; set; }
    }
}