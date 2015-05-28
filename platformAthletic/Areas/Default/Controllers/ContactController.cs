using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels;

namespace platformAthletic.Areas.Default.Controllers
{
    public class ContactController : DefaultController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var feedbackView = new FeedbackView();
            return View(feedbackView);
        }

        [HttpPost]
        public ActionResult Index(FeedbackView feedbackView)
        {
            if (ModelState.IsValid)
            {
                var feedback = (Feedback)ModelMapper.Map(feedbackView, typeof(FeedbackView), typeof(Feedback));

                Repository.CreateFeedback(feedback);
                return View("Thanks");
            }
            return View(feedbackView);
        }

        [HttpGet]
        public ActionResult Thanks()
        {
            return View();
        }
    }
}
