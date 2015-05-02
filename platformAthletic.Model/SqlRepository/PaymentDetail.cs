using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PaymentDetail> PaymentDetails
        {
            get
            {
                return Db.PaymentDetails;
            }
        }

        public bool CreatePaymentDetail(PaymentDetail instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = CurrentDateTime;
                Db.PaymentDetails.InsertOnSubmit(instance);
                Db.PaymentDetails.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ProcessPaymentDetail(PaymentDetail instance)
        {
            var cache = Db.PaymentDetails.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.ProcessedDate = CurrentDateTime;
				Db.PaymentDetails.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool SetResultPaymentDetail(int idPaymentDetail, string result)
        {
            var cache = Db.PaymentDetails.FirstOrDefault(p => p.ID == idPaymentDetail);
            if (cache != null)
            {
                cache.Result = result;
                Db.PaymentDetails.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool RemovePaymentDetail(int idPaymentDetail)
        {
            PaymentDetail instance = Db.PaymentDetails.FirstOrDefault(p => p.ID == idPaymentDetail);
            if (instance != null)
            {
                Db.PaymentDetails.DeleteOnSubmit(instance);
                Db.PaymentDetails.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}