using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class PhaseController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Phases.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var phaseView = new PhaseView();
			return View("Edit", phaseView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  phase = Repository.Phases.FirstOrDefault(p => p.ID == id); 

			if (phase != null) {
				var phaseView = (PhaseView)ModelMapper.Map(phase, typeof(Phase), typeof(PhaseView));
				return View(phaseView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(PhaseView phaseView)
        {
            if (ModelState.IsValid)
            {
                var phase = (Phase)ModelMapper.Map(phaseView, typeof(PhaseView), typeof(Phase));
                if (phase.ID == 0)
                {
                    Repository.CreatePhase(phase);
                }
                else
                {
                    Repository.UpdatePhase(phase);
                }
                return RedirectToAction("Index");
            }
            return View(phaseView);
        }

        public ActionResult Delete(int id)
        {
            var phase = Repository.Phases.FirstOrDefault(p => p.ID == id);
            if (phase != null)
            {
                    Repository.RemovePhase(phase.ID);
            }
			return RedirectToAction("Index");
        }
	}
}