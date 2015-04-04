using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Models.Info;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class TrainingController : AdminController
    {
		public ActionResult Index()
        {

            
			var list = Repository.Trainings.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var trainingView = new TrainingView();
			return View("Edit", trainingView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  training = Repository.Trainings.FirstOrDefault(p => p.ID == id); 

			if (training != null) {
				var trainingView = (TrainingView)ModelMapper.Map(training, typeof(Training), typeof(TrainingView));
				return View(trainingView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(TrainingView trainingView)
        {
            if (ModelState.IsValid)
            {
                var training = (Training)ModelMapper.Map(trainingView, typeof(TrainingView), typeof(Training));
                if (training.ID == 0)
                {
                    Repository.CreateTraining(training);
                }
                else
                {
                    Repository.UpdateTraining(training);
                }
                return RedirectToAction("Index");
            }
            return View(trainingView);
        }

       
        public ActionResult Delete(int id)
        {
            var training = Repository.Trainings.FirstOrDefault(p => p.ID == id);
            if (training != null)
            {
                    Repository.RemoveTraining(training.ID);
            }
			return RedirectToAction("Index");
        }
	}
}