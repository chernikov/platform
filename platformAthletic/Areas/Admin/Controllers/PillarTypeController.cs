using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Tools.Video;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class PillarTypeController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.PillarTypes.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var pillartypeView = new PillarTypeView();
            return View("Edit", pillartypeView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pillartype = Repository.PillarTypes.FirstOrDefault(p => p.ID == id);

            if (pillartype != null)
            {
                var pillartypeView = (PillarTypeView)ModelMapper.Map(pillartype, typeof(PillarType), typeof(PillarTypeView));
                return View(pillartypeView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PillarTypeView pillartypeView)
        {
            if (ModelState.IsValid)
            {
                var pillartype = (PillarType)ModelMapper.Map(pillartypeView, typeof(PillarTypeView), typeof(PillarType));
                if (pillartype.ID == 0)
                {
                    Repository.CreatePillarType(pillartype);
                }
                else
                {
                    Repository.UpdatePillarType(pillartype);
                }
                return RedirectToAction("Index");
            }
            return View(pillartypeView);
        }

        [HttpPost]
        public ActionResult ProcessUrl(string url)
        {
            var code = VideoHelper.GetVideoByUrl(url);

            return Json(new
            {
                result = "ok",
                VideoCode = code
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var pillartype = Repository.PillarTypes.FirstOrDefault(p => p.ID == id);
            if (pillartype != null)
            {
                Repository.RemovePillarType(pillartype.ID);
            }
            return RedirectToAction("Index");
        }
    }
}