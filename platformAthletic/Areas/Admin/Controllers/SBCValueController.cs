using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Global;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class SBCValueController : AdminController
    {
        public ActionResult Index(int page = 1, string search = null)
        {
            var list = Repository.SBCValues;
            if (search != null)
            {
                list = SearchEngine.Search(search, list).AsQueryable();
            }
            var data = new PageableData<SBCValue>();
            data.Init(list, page, "Index");
            ViewData["search"] = search;
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sbcvalue = Repository.SBCValues.FirstOrDefault(p => p.ID == id);

            if (sbcvalue != null)
            {
                var sbcvalueView = (SBCValueView)ModelMapper.Map(sbcvalue, typeof(SBCValue), typeof(SBCValueView));
                return View(sbcvalueView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(SBCValueView sbcvalueView)
        {
            if (ModelState.IsValid)
            {
                var sbcvalue = (SBCValue)ModelMapper.Map(sbcvalueView, typeof(SBCValueView), typeof(SBCValue));
                Repository.UpdateSbcValue(sbcvalue);
                return RedirectToAction("Index");
            }
            return View(sbcvalueView);
        }

        public ActionResult Delete(int id)
        {
            var sbcvalue = Repository.SBCValues.FirstOrDefault(p => p.ID == id);
            if (sbcvalue != null)
            {
                Repository.RemoveSbcValue(sbcvalue.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult SetValues()
        {
            foreach(var user in Repository.Users.ToList()) 
            {
                if (user.Squat != 0)
                {
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Squat, user.Squat);
                }
                if (user.Clean != 0)
                {
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Clean, user.Clean);
                }
                if (user.Bench != 0)
                {
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Bench, user.Bench);
                }
            }
            return Content("OK");
        }
    }
}