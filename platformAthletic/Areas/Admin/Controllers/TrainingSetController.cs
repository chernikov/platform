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
    public class TrainingSetController : AdminController
    {
        private static string SessionFilterName = "TrainingFilter";

        public ActionResult Index()
        {
            FilterTrainingSetInfo filterTrainingSetInfo = null;
            if (Session[SessionFilterName] == null)
            {
                filterTrainingSetInfo = new FilterTrainingSetInfo()
                {
                    DayID = null,
                    PhaseID = null
                };
                Session[SessionFilterName] = filterTrainingSetInfo;
            } else  {
                filterTrainingSetInfo = Session[SessionFilterName] as FilterTrainingSetInfo;
            }
            var list = Repository.TrainingSets;

            if (filterTrainingSetInfo.DayID.HasValue)
            {
                list = list.Where(p => p.DayID == filterTrainingSetInfo.DayID);
            }
            if (filterTrainingSetInfo.PhaseID.HasValue)
            {
                list = list.Where(p => p.PhaseID == filterTrainingSetInfo.PhaseID);
            }
            ViewBag.Search = filterTrainingSetInfo;
            return View(list.OrderBy(p => p.PhaseID).ThenBy(p => p.DayID).ToList());
        }

        
        [HttpPost]
        public ActionResult Index(FilterTrainingSetInfo filterTrainintSetInfo)
        {
            Session[SessionFilterName] = filterTrainintSetInfo;
            return RedirectToAction("Index");
        }

        public ActionResult CreateAll()
        {
            foreach (var training in Repository.Trainings)
            {
                var trainingSet = new TrainingSet();
                Repository.CreateTrainingSet(trainingSet);

                var trainingEquipment = new TrainingEquipment()
                {
                    TrainingSetID = trainingSet.ID,
                    TrainingID = training.ID
                };
                Repository.CreateTrainingEquipment(trainingEquipment);
            }
            return null;
        }

        public ActionResult Create()
        {
            var trainingsetView = new TrainingSetView();
            return View("Edit", trainingsetView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trainingset = Repository.TrainingSets.FirstOrDefault(p => p.ID == id);

            if (trainingset != null)
            {
                var trainingsetView = (TrainingSetView)ModelMapper.Map(trainingset, typeof(TrainingSet), typeof(TrainingSetView));
                return View(trainingsetView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(TrainingSetView trainingsetView)
        {
            if (ModelState.IsValid)
            {
                var trainingset = (TrainingSet)ModelMapper.Map(trainingsetView, typeof(TrainingSetView), typeof(TrainingSet));
                if (trainingset.ID == 0)
                {
                    Repository.CreateTrainingSet(trainingset);
                }
                else
                {
                    Repository.UpdateTrainingSet(trainingset);
                }
                return RedirectToAction("Index");
            }
            return View(trainingsetView);
        }

        public ActionResult TrainingEquipment()
        {
            var trainingEqupmentView = new TrainingEquipmentView();
            return View(trainingEqupmentView);
        }

        public ActionResult Delete(int id)
        {
            var trainingset = Repository.TrainingSets.FirstOrDefault(p => p.ID == id);
            if (trainingset != null)
            {
                Repository.RemoveTrainingSet(trainingset.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateBatch()
        {
            var trainingsetView = new TrainingSetView();
            return View(trainingsetView);
        }

        [HttpPost]
        public ActionResult CreateBatch(TrainingSetView trainingsetView, string batch)
        {
            var arr = batch.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in arr)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var training = Repository.Trainings.FirstOrDefault(p => string.Compare(p.Name, item, true) == 0);
                    if (training == null)
                    {
                        training = new Training()
                        {
                            Name = item
                        };
                        Repository.CreateTraining(training);
                    }

                    var trainingSet = new TrainingSet()
                    {
                        DayID = trainingsetView.DayID,
                        PhaseID = trainingsetView.PhaseID
                    };
                    Repository.CreateTrainingSet(trainingSet);

                    var trainingEquipment = new TrainingEquipment()
                    {
                        TrainingID = training.ID,
                        TrainingSetID = trainingSet.ID
                    };
                    Repository.CreateTrainingEquipment(trainingEquipment);

                }
            }
            return RedirectToAction("Index");
        }

    }
}