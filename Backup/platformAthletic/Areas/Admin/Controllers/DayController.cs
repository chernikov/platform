using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class DayController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Days.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var dayView = new DayView();
			return View("Edit", dayView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  day = Repository.Days.FirstOrDefault(p => p.ID == id); 

			if (day != null) {
				var dayView = (DayView)ModelMapper.Map(day, typeof(Day), typeof(DayView));
				return View(dayView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(DayView dayView)
        {
            if (ModelState.IsValid)
            {
                var day = (Day)ModelMapper.Map(dayView, typeof(DayView), typeof(Day));
                if (day.ID == 0)
                {
                    Repository.CreateDay(day);
                }
                else
                {
                    Repository.UpdateDay(day);
                }
                return RedirectToAction("Index");
            }
            return View(dayView);
        }

        public ActionResult Delete(int id)
        {
            var day = Repository.Days.FirstOrDefault(p => p.ID == id);
            if (day != null)
            {
                    Repository.RemoveDay(day.ID);
            }
			return RedirectToAction("Index");
        }
	}
}