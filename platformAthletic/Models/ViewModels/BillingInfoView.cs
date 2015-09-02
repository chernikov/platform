using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Helpers;


namespace platformAthletic.Models.ViewModels
{ 
	public class BillingInfoView
    {

        public enum CardTypeEnum
        {
            Visa = 0x01, 
            MasterCard = 0x02, 
       /*     AmericanExpress = 0x03, */
            Discovery = 0x04
        }

        public int ID { get; set; }

        public int UserID { get; set; }

		public bool AutoDebit {get; set; }

        [Required(ErrorMessage="Enter Cardholder Name")]
        [StringLength(500, ErrorMessage = "The Name On Card can not exceed 500 characters")]
		public string NameOnCard {get; set; }

        [Required(ErrorMessage = "Enter Billing Address")]
		public string BillingAddress {get; set; }

        [Required(ErrorMessage = "Enter City")]
        [StringLength(50, ErrorMessage = "The City can not exceed 50 characters")]
		public string City {get; set; }

		public int StateID {get; set; }

        [Required(ErrorMessage = "Enter Zip")]
        [StringLength(50, ErrorMessage = "The Zip Code can not exceed 50 characters")]
        public string ZipCode { get; set; }

		public int CardType {get; set; }

        [Required(ErrorMessage = "Enter Card Number")]
        [StringLength(50, ErrorMessage = "The Card Number can not exceed 50 characters")]
		public string CreditCardNumber {get; set; }

        public string HiddenCreditCardNumber
        {
            get
            {
                if (CreditCardNumber != null && CreditCardNumber.Length > 12)
                {
                    return "**** **** **** " + CreditCardNumber.Substring(12);
                }
                return string.Empty;
            }
        }

        [Required(ErrorMessage = "Enter CVC")]
        [StringLength(50, ErrorMessage = "The CVC can not exceed 50 characters")]
		public string CVC {get; set; }

        [StringLength(50, ErrorMessage = "The Referral Code can not exceed 50 characters")]
		public string ReferralCode {get; set; }

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
                                                        Selected = p.ID == StateID
        	                                        });
        	}
        }

        public IEnumerable<SelectListItem> SelectListCardType
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)CardTypeEnum.Visa).ToString(), Text = "Visa", Selected = CardType == (int)CardTypeEnum.Visa };
                yield return new SelectListItem() { Value = ((int)CardTypeEnum.MasterCard).ToString(), Text = "MasterCard", Selected = CardType == (int)CardTypeEnum.MasterCard };
           /*     yield return new SelectListItem() { Value = ((int)CardTypeEnum.AmericanExpress).ToString(), Text = "AmEx", Selected = CardType == (int)CardTypeEnum.AmericanExpress };*/
                yield return new SelectListItem() { Value = ((int)CardTypeEnum.Discovery).ToString(), Text = "Discovery", Selected = CardType == (int)CardTypeEnum.Discovery };
            }
        }

        public int ExpirationMonth { get; set; }

        public IEnumerable<SelectListItem> ExpirationMonthSelectList
        {
            get
            {
                for (int i = 1; i < 13; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ExpirationMonth == i
                    };
                }
            }
        }

        public int ExpirationYear { get; set; }

        public IEnumerable<SelectListItem> ExpirationYearSelectList
        {
            get
            {
                for (int i = DateTime.Now.Current().Year; i < DateTime.Now.Year + 10; i++)
                {
                    yield return new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString(),
                        Selected = ExpirationYear == i
                    };
                }
            }
        }
    }
}