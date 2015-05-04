using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Tools.Video;
using System.Net;
using System.IO;
using platformAthletic.Tools;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class PillarTypeController : AdminController
    {
        protected string DestinationDir = "Media/files/previews/";

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
                var pillarType = (PillarType)ModelMapper.Map(pillartypeView, typeof(PillarTypeView), typeof(PillarType));
                pillarType.Preview = GetPreview(pillarType.VideoUrl);
                pillarType.VideoCode = VideoHelper.GetVideoByUrl(pillarType.VideoUrl);

                if (pillarType.ID == 0)
                {
                    Repository.CreatePillarType(pillarType);
                }
                else
                {
                    Repository.UpdatePillarType(pillarType);
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
            var pillarType = Repository.PillarTypes.FirstOrDefault(p => p.ID == id);
            if (pillarType != null)
            {
                Repository.RemovePillarType(pillarType.ID);
            }
            return RedirectToAction("Index");
        }

        private string GetPreview(string videoUrl)
        {
            var url = VideoHelper.GetVideoThumbByUrl(videoUrl);
            var webClient = new WebClient();
            var bytes = webClient.DownloadData(url);
            var stream = new MemoryStream(bytes);

            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
            var urlFile = "/" + Path.Combine(DestinationDir, uFile);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);
            return urlFile;
        }
    }
}