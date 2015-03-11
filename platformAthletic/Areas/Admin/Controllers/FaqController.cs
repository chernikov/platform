using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class FaqController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Faqs.OrderBy(p => p.OrderBy).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var faqView = new FaqView();
            return View("Edit", faqView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var faq = Repository.Faqs.FirstOrDefault(p => p.ID == id);

            if (faq != null)
            {
                var faqView = (FaqView)ModelMapper.Map(faq, typeof(Faq), typeof(FaqView));
                return View(faqView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FaqView faqView)
        {
            if (ModelState.IsValid)
            {
                var faq = (Faq)ModelMapper.Map(faqView, typeof(FaqView), typeof(Faq));
                if (faq.ID == 0)
                {
                    Repository.CreateFaq(faq);
                }
                else
                {
                    Repository.UpdateFaq(faq);
                }
                return RedirectToAction("Index");
            }
            return View(faqView);
        }

        public ActionResult Delete(int id)
        {
            var faq = Repository.Faqs.FirstOrDefault(p => p.ID == id);
            if (faq != null)
            {
                Repository.RemoveFaq(faq.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AjaxFaqOrder(int id, int replaceTo)
        {
            if (Repository.MoveFaq(id, replaceTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }
    }
}