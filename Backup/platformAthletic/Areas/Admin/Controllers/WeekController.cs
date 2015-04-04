using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class WeekController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Weeks.OrderBy(p => p.Phase.Cycle.SeasonID).ThenBy(p => p.Number).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var weekView = new WeekView();
            return View("Edit", weekView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var week = Repository.Weeks.FirstOrDefault(p => p.ID == id);

            if (week != null)
            {
                var weekView = (WeekView)ModelMapper.Map(week, typeof(Week), typeof(WeekView));
                return View(weekView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(WeekView weekView)
        {
            if (ModelState.IsValid)
            {
                var week = (Week)ModelMapper.Map(weekView, typeof(WeekView), typeof(Week));
                if (week.ID == 0)
                {
                    Repository.CreateWeek(week);
                    var macrocycle = Repository.Macrocycles.First(p => p.WeekID == week.ID);

                    foreach (var day in Repository.Days)
                    {
                        var trainingDay = new TrainingDay()
                        {
                            DayID = day.ID,
                            WeekID = week.ID,
                            MacrocycleID = macrocycle.ID
                        };
                        Repository.CreateTrainingDay(trainingDay);
                    }
                }
                else
                {
                    Repository.UpdateWeek(week);
                }
                return RedirectToAction("Index");
            }
            return View(weekView);
        }

        public ActionResult Delete(int id)
        {
            var week = Repository.Weeks.FirstOrDefault(p => p.ID == id);
            if (week != null)
            {
                Repository.RemoveWeek(week.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult FixMacrocycle(int id)
        {

            var week = Repository.Weeks.FirstOrDefault(p => p.ID == id);
            if (week != null)
            {
                Repository.CreateMacrocycle(new Macrocycle()
                {
                    Name = week.FullName,
                    WeekID = week.ID
                });
            }
            return RedirectToAction("Index");
        }
    }
}