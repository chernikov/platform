using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using System.IO;
using platformAthletic.Tools;
using System.Drawing;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class BannerController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Banners.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var bannerView = new BannerView()
            {
                InRotation = true
            };
            return View("Edit", bannerView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var banner = Repository.Banners.FirstOrDefault(p => p.ID == id);

            if (banner != null)
            {
                var bannerView = (BannerView)ModelMapper.Map(banner, typeof(Banner), typeof(BannerView));
                return View(bannerView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(BannerView bannerView)
        {
            if (ModelState.IsValid)
            {
                var banner = (Banner)ModelMapper.Map(bannerView, typeof(BannerView), typeof(Banner));
                if (banner.ID == 0)
                {
                    Repository.CreateBanner(banner);
                }
                else
                {
                    Repository.UpdateBanner(banner);
                }
                return RedirectToAction("Index");
            }
            return View(bannerView);
        }

        public ActionResult Delete(int id)
        {
            var banner = Repository.Banners.FirstOrDefault(p => p.ID == id);
            if (banner != null)
            {
                Repository.RemoveBanner(banner.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult MakePreview(string sourcePath, int bannerPlaceID)
        {
            var bannerPlace = Repository.BannerPlaces.FirstOrDefault(p => p.ID == bannerPlaceID);
            if (bannerPlace != null)
            {
                var size = new Size(bannerPlace.Width, bannerPlace.Height);
                var previewPath = sourcePath.GetPreviewPath(string.Format("_{0}_{1}", size.Width, size.Height), ".jpg");

                using (var file = new FileStream(Server.MapPath(sourcePath), FileMode.Open, System.IO.FileAccess.Read))
                {
                    PreviewCreator.CreateAndSavePreview(file, size, Server.MapPath(previewPath), Brushes.White);
                }

                return Json(new
                {
                    result = "ok",
                    data = previewPath
                });
            }
            return Json(new { result = "error" });
        }
    }
}