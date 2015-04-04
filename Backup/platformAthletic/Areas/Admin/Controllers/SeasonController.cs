using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class SeasonController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Seasons.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var seasonView = new SeasonView();
			return View("Edit", seasonView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  season = Repository.Seasons.FirstOrDefault(p => p.ID == id); 

			if (season != null) {
				var seasonView = (SeasonView)ModelMapper.Map(season, typeof(Season), typeof(SeasonView));
				return View(seasonView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(SeasonView seasonView)
        {
            if (ModelState.IsValid)
            {
                var season = (Season)ModelMapper.Map(seasonView, typeof(SeasonView), typeof(Season));
                if (season.ID == 0)
                {
                    Repository.CreateSeason(season);
                }
                else
                {
                    Repository.UpdateSeason(season);
                }
                return RedirectToAction("Index");
            }
            return View(seasonView);
        }

        public ActionResult Delete(int id)
        {
            var season = Repository.Seasons.FirstOrDefault(p => p.ID == id);
            if (season != null)
            {
                    Repository.RemoveSeason(season.ID);
            }
			return RedirectToAction("Index");
        }
	}
}