using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class PeopleSayingController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.PeopleSayings.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var peoplesayingView = new PeopleSayingView();
            return View("Edit", peoplesayingView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var peoplesaying = Repository.PeopleSayings.FirstOrDefault(p => p.ID == id);

            if (peoplesaying != null)
            {
                var peoplesayingView = (PeopleSayingView)ModelMapper.Map(peoplesaying, typeof(PeopleSaying), typeof(PeopleSayingView));
                return View(peoplesayingView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(PeopleSayingView peoplesayingView)
        {
            if (ModelState.IsValid)
            {
                var peoplesaying = (PeopleSaying)ModelMapper.Map(peoplesayingView, typeof(PeopleSayingView), typeof(PeopleSaying));
                if (peoplesaying.ID == 0)
                {
                    Repository.CreatePeopleSaying(peoplesaying);
                }
                else
                {
                    Repository.UpdatePeopleSaying(peoplesaying);
                }
                return RedirectToAction("Index");
            }
            return View(peoplesayingView);
        }

        public ActionResult Delete(int id)
        {
            var peoplesaying = Repository.PeopleSayings.FirstOrDefault(p => p.ID == id);
            if (peoplesaying != null)
            {
                Repository.RemovePeopleSaying(peoplesaying.ID);
            }
            return RedirectToAction("Index");
        }
    }
}