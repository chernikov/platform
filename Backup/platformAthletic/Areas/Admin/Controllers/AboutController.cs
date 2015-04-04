using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class AboutController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Abouts.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var aboutView = new AboutView();
			return View("Edit", aboutView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  about = Repository.Abouts.FirstOrDefault(p => p.ID == id); 

			if (about != null) {
				var aboutView = (AboutView)ModelMapper.Map(about, typeof(About), typeof(AboutView));
				return View(aboutView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(AboutView aboutView)
        {
            if (ModelState.IsValid)
            {
                var about = (About)ModelMapper.Map(aboutView, typeof(AboutView), typeof(About));
                if (about.ID == 0)
                {
                    Repository.CreateAbout(about);
                }
                else
                {
                    Repository.UpdateAbout(about);
                }
                return RedirectToAction("Index");
            }
            return View(aboutView);
        }

        public ActionResult Delete(int id)
        {
            var about = Repository.Abouts.FirstOrDefault(p => p.ID == id);
            if (about != null)
            {
                    Repository.RemoveAbout(about.ID);
            }
			return RedirectToAction("Index");
        }
	}
}