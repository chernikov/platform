using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BillingInfo> BillingInfoes
        {
            get
            {
                return Db.BillingInfos;
            }
        }

        public bool CreateBillingInfo(BillingInfo instance)
        {
            if (instance.ID == 0)
            {
                instance.AutoDebit = true;
                Db.BillingInfos.InsertOnSubmit(instance);
                Db.BillingInfos.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public BillingInfo UpdateBillingInfo(BillingInfo instance)
        {
            var cache = Db.BillingInfos.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
			
				cache.NameOnCard = instance.NameOnCard;
				cache.BillingAddress = instance.BillingAddress;
				cache.City = instance.City;
				cache.StateID = instance.StateID;
				cache.ZipCode = instance.ZipCode;
				cache.CardType = instance.CardType;
                if (!instance.CreditCardNumber.StartsWith("*"))
                {
                    cache.CreditCardNumber = instance.CreditCardNumber;
                }
				cache.ExparationDate = instance.ExparationDate;
				cache.CVC = instance.CVC;
                Db.BillingInfos.Context.SubmitChanges();
                return cache;
            }

            return null;
        }

        public bool CancelAutoDebit(int idBillingInfo)
        {
            var instance = Db.BillingInfos.FirstOrDefault(p => p.ID == idBillingInfo);
            if (instance != null)
            {
                instance.AutoDebit = false;
                Db.BillingInfos.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveBillingInfo(int idBillingInfo)
        {
            var instance = Db.BillingInfos.FirstOrDefault(p => p.ID == idBillingInfo);
            if (instance != null)
            {
                Db.BillingInfos.DeleteOnSubmit(instance);
                Db.BillingInfos.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}