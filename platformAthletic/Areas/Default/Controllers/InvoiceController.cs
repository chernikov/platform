using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;
using platformAthletic.Model;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach")]
    public class InvoiceController : DefaultController
    {
        public ActionResult Index()
        {
            Invoice invoice = Repository.Invoices.OrderByDescending(p => p.ID).FirstOrDefault(p => p.UserID == CurrentUser.ID);
            var totalSum = Double.Parse(Repository.Settings.First(p => p.Name == "TeamPrice").Value);
               
            if (invoice == null)
            {
                /*var billingInfo = Repository.BillingInfoes.FirstOrDefault(p => p.UserID == CurrentUser.ID);
                if (billingInfo != null)*/
                invoice = new Invoice()
                {
                    UserID = CurrentUser.ID,
                    City = "", 
                    ZipCode = "",
                    StateID = CurrentUser.OwnTeam.StateID,
                    PhoneNumber = CurrentUser.PhoneNumber,
                    NameOfOrganization = CurrentUser.OwnTeam.Name,
                };
                invoice.TotalSum = totalSum;
                Repository.CreateInvoice(invoice);
            }
            else
            {
                if (invoice.TotalSum != totalSum)
                {
                    invoice.TotalSum = totalSum;
                    Repository.CreateInvoice(invoice);
                }
            }
            if (invoice != null)
            {
                return View(invoice);
            }
            return null;
        }

    }
}
