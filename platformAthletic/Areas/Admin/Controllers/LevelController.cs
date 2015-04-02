using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class LevelController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Levels.ToList();
            return View(list);
        }


        public ActionResult Create()
        {
            var levelView = new LevelView();
            return View("Edit", levelView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var level = Repository.Levels.FirstOrDefault(p => p.ID == id);

            if (level != null)
            {
                var levelView = (LevelView)ModelMapper.Map(level, typeof(Level), typeof(LevelView));
                return View(levelView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(LevelView levelView)
        {
            if (ModelState.IsValid)
            {
                var level = (Level)ModelMapper.Map(levelView, typeof(LevelView), typeof(Level));
                if (level.ID == 0)
                {
                    Repository.CreateLevel(level);
                }
                else
                {
                    Repository.UpdateLevel(level);
                }
                return RedirectToAction("Index");
            }
            return View(levelView);
        }

        public ActionResult Delete(int id)
        {
            var level = Repository.Levels.FirstOrDefault(p => p.ID == id);
            if (level != null)
            {
                Repository.RemoveLevel(level.ID);
            }
            return RedirectToAction("Index");
        }
    }
}
