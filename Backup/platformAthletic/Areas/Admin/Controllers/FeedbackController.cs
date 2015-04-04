using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class FeedbackController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Feedbacks.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Item(int id)
        {
            var feedback = Repository.Feedbacks.FirstOrDefault(p => p.ID == id);

            if (feedback != null)
            {
                Repository.ReadFeedback(feedback);
                return View(feedback);
            }
            return RedirectToNotFoundPage;
        }


        public ActionResult Delete(int id)
        {
            var feedback = Repository.Feedbacks.FirstOrDefault(p => p.ID == id);
            if (feedback != null)
            {
                Repository.RemoveFeedback(feedback.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Count()
        {
            return View("Count", Repository.Feedbacks.Count(p => !p.IsReaded));
        }
    }
}