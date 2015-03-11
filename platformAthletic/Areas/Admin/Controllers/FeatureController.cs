using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class FeatureController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.FeatureCatalogs.OrderBy(p => p.OrderBy).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var featurecatalogView = new FeatureCatalogView();
            return View("Edit", featurecatalogView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var featurecatalog = Repository.FeatureCatalogs.FirstOrDefault(p => p.ID == id);

            if (featurecatalog != null)
            {
                var featurecatalogView = (FeatureCatalogView)ModelMapper.Map(featurecatalog, typeof(FeatureCatalog), typeof(FeatureCatalogView));
                return View(featurecatalogView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FeatureCatalogView featurecatalogView)
        {
            if (ModelState.IsValid)
            {
                var featurecatalog = (FeatureCatalog)ModelMapper.Map(featurecatalogView, typeof(FeatureCatalogView), typeof(FeatureCatalog));
                if (featurecatalog.ID == 0)
                {
                    Repository.CreateFeatureCatalog(featurecatalog);
                }
                else
                {
                    Repository.UpdateFeatureCatalog(featurecatalog);
                }
                return RedirectToAction("Index");
            }
            return View(featurecatalogView);
        }

        public ActionResult Delete(int id)
        {
            var featurecatalog = Repository.FeatureCatalogs.FirstOrDefault(p => p.ID == id);
            if (featurecatalog != null)
            {
                Repository.RemoveFeatureCatalog(featurecatalog.ID);
            }
            return RedirectToAction("Index");
        }


        public ActionResult CreateText(int id)
        {
            var featuretextView = new FeatureTextView()
            {
                FeatureCatalogID = id
            };
            return View("EditText", featuretextView);
        }

        [HttpGet]
        public ActionResult EditText(int id)
        {
            var featuretext = Repository.FeatureTexts.FirstOrDefault(p => p.ID == id);

            if (featuretext != null)
            {
                var featuretextView = (FeatureTextView)ModelMapper.Map(featuretext, typeof(FeatureText), typeof(FeatureTextView));
                return View(featuretextView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditText(FeatureTextView featuretextView)
        {
            if (ModelState.IsValid)
            {
                var featuretext = (FeatureText)ModelMapper.Map(featuretextView, typeof(FeatureTextView), typeof(FeatureText));
                if (featuretext.ID == 0)
                {
                    Repository.CreateFeatureText(featuretext);
                }
                else
                {
                    Repository.UpdateFeatureText(featuretext);
                }
                return RedirectToAction("Index");
            }
            return View(featuretextView);
        }

        public ActionResult DeleteText(int id)
        {
            var featuretext = Repository.FeatureTexts.FirstOrDefault(p => p.ID == id);
            if (featuretext != null)
            {
                Repository.RemoveFeatureText(featuretext.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AjaxFeatureCatalogOrder(int id, int replaceTo)
        {
            if (Repository.MoveFeatureCatalog(id, replaceTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult AjaxFeatureTextOrder(int id, int replaceTo)
        {
            if (Repository.MoveFeatureText(id, replaceTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public JsonResult AjaxFeatureTextMove(int id, int moveTo)
        {
            if (Repository.ChangeParentFeatureText(id, moveTo))
            {
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }
    }
}