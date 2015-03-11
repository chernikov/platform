using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class MacrocycleController : AdminController
    {
        public ActionResult Init()
        {
            foreach (var trainingDay in Repository.TrainingDays.ToList())
            {
                var weekID = trainingDay.WeekID;

                var week = Repository.Weeks.FirstOrDefault(p => p.ID == weekID);

                if (week != null)
                {
                    Macrocycle macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.WeekID == week.ID);
                    if (macrocycle == null)
                    {
                        macrocycle = new Macrocycle()
                        {
                            WeekID = week.ID,
                            Name = week.Phase.Cycle.Name + " " + week.Phase.Name + " " + week.Name
                        };

                        Repository.CreateMacrocycle(macrocycle);
                    }
                    else
                    {
                        macrocycle.Name = week.Phase.Cycle.Name + " " + week.Phase.Name + " " + week.Name;
                        Repository.UpdateMacrocycle(macrocycle);
                    }
                    trainingDay.MacrocycleID = macrocycle.ID;
                    Repository.UpdateTrainingDay(trainingDay);
                }
            }

            return Content("OK");
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ActionResult Index(bool offSeason = true)
        {
            ViewBag.Offseason = offSeason;
            var list = Repository.Macrocycles.Where(p => p.Week.Phase.Cycle.Season.Type == (int)(offSeason ? (Season.TypeEnum.OffSeason) : (Season.TypeEnum.InSeason))).OrderBy(p => p.ID);
            return View(list);
        }

        [HttpGet]
        public ActionResult Create(bool offSeason)
        {
            var macrocycle = new MacrocycleView()
            {
                SeasonType = offSeason ? 0 : 1
            };

            return View(macrocycle);
        }

        [HttpPost]
        public ActionResult Create(MacrocycleView macrocycleView)
        {
            if (ModelState.IsValid)
            {
                var macrocycle = (Macrocycle)ModelMapper.Map(macrocycleView, typeof(MacrocycleView), typeof(Macrocycle));

                Repository.CreateMacrocycle(macrocycle);

                foreach (var day in Repository.Days.ToList())
                {
                    var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.WeekID == macrocycle.WeekID &&
                        p.DayID == day.ID);
                    if (trainingDay != null)
                    {
                        trainingDay.MacrocycleID = macrocycle.ID;
                        Repository.UpdateTrainingDay(trainingDay);
                    }
                }
                return View("_OK");
            }
            return View(macrocycleView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);

            if (macrocycle != null)
            {
                ViewBag.Macrocycle = macrocycle;
                var listOfTrainingDay = macrocycle.TrainingDays.OrderBy(p => p.DayID).ToList();

                var listOfTrainingDayView = new List<TrainingDayView>();

                foreach (var trainingDay in listOfTrainingDay)
                {
                    listOfTrainingDayView.Add((TrainingDayView)ModelMapper.Map(trainingDay, typeof(TrainingDay), typeof(TrainingDayView)));
                }
                return View(listOfTrainingDayView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpGet]
        public ActionResult EditName(int id)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);

            if (macrocycle != null)
            {
                var macrocycleView = (MacrocycleView)ModelMapper.Map(macrocycle, typeof(Macrocycle), typeof(MacrocycleView));

                return View(macrocycleView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult EditName(MacrocycleView macrocycleView)
        {
            if (ModelState.IsValid)
            {
                var macrocycle = (Macrocycle)ModelMapper.Map(macrocycleView, typeof(MacrocycleView), typeof(Macrocycle));
                Repository.UpdateMacrocycleName(macrocycle);
                return RedirectToAction("Index");
            }
            return View(macrocycleView);
        }

        [HttpGet]
        public ActionResult EditCellInfo(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpPost]
        public ActionResult EditCellInfo(TrainingDayCellView trainingDayCellView)
        {
            if (ModelState.IsValid)
            {
                var trainingDayCell = (TrainingDayCell)ModelMapper.Map(trainingDayCellView, typeof(TrainingDayCellView), typeof(TrainingDayCell));

                if (trainingDayCell.ID == 0)
                {
                    Repository.CreateTrainingDayCell(trainingDayCell);
                }
                else
                {
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            return Json(new { result = "ok" });
        }

        [HttpGet]
        public ActionResult EditCellExercise(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpGet]
        public ActionResult EditCellValue(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpPost]
        public ActionResult EditCellExercise(TrainingDayCellView trainingDayCellView)
        {
            if (ModelState.IsValid)
            {
                var trainingDayCell = (TrainingDayCell)ModelMapper.Map(trainingDayCellView, typeof(TrainingDayCellView), typeof(TrainingDayCell));

                if (trainingDayCell.ID == 0)
                {
                    Repository.CreateTrainingDayCell(trainingDayCell);
                }
                else
                {
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            return Json(new { result = "ok" });
        }

        [HttpPost]
        public ActionResult ClearCell(int cellID, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellID && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                Repository.RemoveTrainingDayCell(trainingDayCell.ID);
            }
            return Json(new { result = "ok" });
        }

        public ActionResult GetCellInfo(int cellID, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellID && p.TrainingDayID == trainingDayID);

            if (trainingDayCell != null)
            {
                return Json(new { result = "ok", data = trainingDayCell.Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWeekInfo(int id)
        {
            var week = Repository.Weeks.FirstOrDefault(p => p.ID == id);
            if (week != null)
            {
                return Content(week.FullName);
            }
            return null;
        }

        public ActionResult GetWeekList(int id)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);

            if (macrocycle != null)
            {
                var week = macrocycle.Week;
                var season = week.Phase.Cycle.Season;
                var list = season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).ToList();
                return View(SelectList(list, week.ID));
            }
            return null;
        }

        public IEnumerable<SelectListItem> SelectList(List<Week> list, int selected)
        {
            foreach (var week in list)
            {
                yield return new SelectListItem()
                {
                    Value = week.ID.ToString(),
                    Text = week.Number.ToString(),
                    Selected = week.ID == selected
                };
            }
        }


        public ActionResult SetWeek(int id, int val)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);
            if (macrocycle != null)
            {
                foreach (var trainingDay in macrocycle.TrainingDays.ToList())
                {
                    trainingDay.MacrocycleID = null;
                    Repository.UpdateTrainingDay(trainingDay);
                }
                macrocycle.WeekID = val;
                Repository.UpdateMacrocycle(macrocycle);

                foreach (var day in Repository.Days.ToList())
                {
                    var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.WeekID == macrocycle.WeekID &&
                        p.DayID == day.ID);
                    if (trainingDay != null)
                    {
                        trainingDay.MacrocycleID = macrocycle.ID;
                        Repository.UpdateTrainingDay(trainingDay);
                    }
                }
                return Content(macrocycle.Week.Number.ToString());
            }
            return null;
        }

        public ActionResult GetWeekName(int id)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);

            if (macrocycle != null)
            {
                return View(macrocycle);
            }
            return null;
        }

        public ActionResult SetWeekName(int id, string name)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);
            if (macrocycle != null)
            {
                macrocycle.Name = name;
                Repository.UpdateMacrocycle(macrocycle);

                return Content(macrocycle.Name);
            }
            return null;
        }

        //public ActionResult Delete(int id)
        //{
        //    var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == id);
        //    if (macrocycle != null)
        //    {
        //        Repository.RemoveMacrocycle(macrocycle.ID);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}

