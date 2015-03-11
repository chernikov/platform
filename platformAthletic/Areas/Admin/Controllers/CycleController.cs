using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class CycleController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Cycles.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var cycleView = new CycleView();
			return View("Edit", cycleView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  cycle = Repository.Cycles.FirstOrDefault(p => p.ID == id); 

			if (cycle != null) {
				var cycleView = (CycleView)ModelMapper.Map(cycle, typeof(Cycle), typeof(CycleView));
				return View(cycleView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(CycleView cycleView)
        {
            if (ModelState.IsValid)
            {
                var cycle = (Cycle)ModelMapper.Map(cycleView, typeof(CycleView), typeof(Cycle));
                if (cycle.ID == 0)
                {
                    Repository.CreateCycle(cycle);
                }
                else
                {
                    Repository.UpdateCycle(cycle);
                }
                return RedirectToAction("Index");
            }
            return View(cycleView);
        }

        public ActionResult Delete(int id)
        {
            var cycle = Repository.Cycles.FirstOrDefault(p => p.ID == id);
            if (cycle != null)
            {
                    Repository.RemoveCycle(cycle.ID);
            }
			return RedirectToAction("Index");
        }
	}
}