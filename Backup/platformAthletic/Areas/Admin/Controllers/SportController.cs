using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class SportController : AdminController
    {
        //
        // GET: /Admin/Sport/

        public ActionResult Index()
        {
            var list = Repository.Sports.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var sportView = new SportView();
            return View("Edit", sportView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sport = Repository.Sports.FirstOrDefault(p => p.ID == id);

            if (sport != null)
            {
                var sportView = (SportView)ModelMapper.Map(sport, typeof(Sport), typeof(SportView));
                return View(sportView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(SportView sportView)
        {
            if (ModelState.IsValid)
            {
                var sport = (Sport)ModelMapper.Map(sportView, typeof(SportView), typeof(Sport));
                if (sport.ID == 0)
                {
                    Repository.CreateSport(sport);
                }
                else
                {
                    Repository.UpdateSport(sport);
                }
                return RedirectToAction("Index");
            }
            return View(sportView);
        }

        public ActionResult Delete(int id)
        {
            var sport = Repository.Sports.FirstOrDefault(p => p.ID == id);
            if (sport != null)
            {
                Repository.RemoveSport(sport.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
