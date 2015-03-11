using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using platformAthletic.Tools;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Invoice> Invoices
        {
            get
            {
                return Db.Invoices;
            }
        }

        public bool CreateInvoice(Invoice instance)
        {
            if (instance.ID == 0)
            {
                var lastID = Db.Invoices.OrderByDescending(p => p.ID).Select(p => p.ID).FirstOrDefault() + 1;
                lastID = lastID * 12 + 100000;
                instance.DateSent = DateTime.Now;
                instance.DateDue = DateTime.Now.AddMonths(1);
                instance.Code = string.Format("INV-{0}", lastID.ToString().Insert(3, "-")); //TODO: Generate Invoice Code;
                Db.Invoices.InsertOnSubmit(instance);
                Db.Invoices.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateInvoice(Invoice instance)
        {
            var cache = Db.Invoices.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Code = instance.Code;
                cache.NameOfOrganization = instance.NameOfOrganization;
                cache.City = instance.City;
                cache.StateID = instance.StateID;
                cache.ZipCode = instance.ZipCode;
                cache.PhoneNumber = instance.PhoneNumber;
                cache.DateSent = instance.DateSent;
                cache.DateDue = instance.DateDue;
                Db.Invoices.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveInvoice(int idInvoice)
        {
            Invoice instance = Db.Invoices.FirstOrDefault(p => p.ID == idInvoice);
            if (instance != null)
            {
                Db.Invoices.DeleteOnSubmit(instance);
                Db.Invoices.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}