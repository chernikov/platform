using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;

namespace platformAthletic.Areas.Default.Controllers
{
    public class FeatureController : DefaultController
    {
        public ActionResult Index(int? id = null)
        {
            FeatureText feature = null;
            if (id.HasValue) {
                feature = Repository.FeatureTexts.FirstOrDefault(p => p.ID == id.Value);
            } 
            if (feature == null) 
            {
                var featureCatalog = Repository.FeatureCatalogs.OrderBy(p => p.OrderBy).Where(p => p.FeatureTexts.Any()).FirstOrDefault();
                if (featureCatalog != null) 
                {
                    feature = featureCatalog.FeatureTexts.OrderBy(p => p.OrderBy).FirstOrDefault();
                }
            }
            return View(feature);
        }

        public ActionResult MenuFeature(int id)
        {
            var list = Repository.FeatureCatalogs.OrderBy(p => p.OrderBy);
            ViewBag.FeatureTextID = id;
            return View(list);
        }
    }
}
