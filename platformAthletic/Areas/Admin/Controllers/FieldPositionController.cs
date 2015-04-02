using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class FieldPositionController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.FieldPositions.ToList();
            return View(list);
        }
        
        public ActionResult Create()
        {
            var fieldpositionView = new FieldPositionView();
            return View("Edit", fieldpositionView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fieldposition = Repository.FieldPositions.FirstOrDefault(p => p.ID == id);

            if (fieldposition != null)
            {
                var fieldpositionView = (FieldPositionView)ModelMapper.Map(fieldposition, typeof(FieldPosition), typeof(FieldPositionView));
                return View(fieldpositionView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(FieldPositionView fieldpositionView)
        {
            if (ModelState.IsValid)
            {
                var fieldposition = (FieldPosition)ModelMapper.Map(fieldpositionView, typeof(FieldPositionView), typeof(FieldPosition));
                if (fieldposition.ID == 0)
                {
                    Repository.CreateFieldPosition(fieldposition);
                }
                else
                {
                    Repository.UpdateFieldPosition(fieldposition);
                }
                return RedirectToAction("Index");
            }
            return View(fieldpositionView);
        }

        public ActionResult Delete(int id)
        {
            var fieldposition = Repository.FieldPositions.FirstOrDefault(p => p.ID == id);
            if (fieldposition != null)
            {
                Repository.RemoveFieldPosition(fieldposition.ID);
            }
            return RedirectToAction("Index");
        }
    }
}