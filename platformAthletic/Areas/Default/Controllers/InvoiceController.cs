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
            else if (invoice.DateSent.AddYears(1) < DateTime.Now.Current() && invoice.TotalSum != totalSum)
            {
                var newInvoice = new Invoice()
                {
                    UserID = invoice.UserID,
                    City = invoice.City,
                    Code = invoice.Code,
                    StateID = invoice.StateID,
                    PhoneNumber = invoice.PhoneNumber,
                    NameOfOrganization = invoice.NameOfOrganization,
                    TotalSum = totalSum,
                    ZipCode = invoice.ZipCode
                };
                Repository.CreateInvoice(newInvoice);
                return View(newInvoice);
            }
            if (invoice != null)
            {
                return View(invoice);
            }
            return null;
        }

    }
}
