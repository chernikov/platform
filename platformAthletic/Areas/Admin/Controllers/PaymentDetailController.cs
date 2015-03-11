using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class PaymentDetailController : AdminController
    {
        public ActionResult Index(int page = 1)
        {
            var list = Repository.PaymentDetails.Where(p => p.ProcessedDate.HasValue);
            var data = new PageableData<PaymentDetail>();
            data.Init(list, page, "Index");
            return View(data);
        }


        [HttpGet]
        public ActionResult Detail(int id)
        {
            var paymentdetail = Repository.PaymentDetails.FirstOrDefault(p => p.ID == id);

            if (paymentdetail != null)
            {
                return View(paymentdetail);
            }
            return RedirectToNotFoundPage;
        }

    }
}