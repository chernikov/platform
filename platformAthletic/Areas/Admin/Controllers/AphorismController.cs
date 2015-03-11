using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class AphorismController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Aphorisms.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var aphorismView = new AphorismView();
			return View("Edit", aphorismView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  aphorism = Repository.Aphorisms.FirstOrDefault(p => p.ID == id); 

			if (aphorism != null) {
				var aphorismView = (AphorismView)ModelMapper.Map(aphorism, typeof(Aphorism), typeof(AphorismView));
				return View(aphorismView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(AphorismView aphorismView)
        {
            if (ModelState.IsValid)
            {
                var aphorism = (Aphorism)ModelMapper.Map(aphorismView, typeof(AphorismView), typeof(Aphorism));
                if (aphorism.ID == 0)
                {
                    Repository.CreateAphorism(aphorism);
                }
                else
                {
                    Repository.UpdateAphorism(aphorism);
                }
                return RedirectToAction("Index");
            }
            return View(aphorismView);
        }

        public ActionResult Delete(int id)
        {
            var aphorism = Repository.Aphorisms.FirstOrDefault(p => p.ID == id);
            if (aphorism != null)
            {
                    Repository.RemoveAphorism(aphorism.ID);
            }
			return RedirectToAction("Index");
        }
	}
}