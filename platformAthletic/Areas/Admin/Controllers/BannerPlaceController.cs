using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class BannerPlaceController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.BannerPlaces.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var bannerplaceView = new BannerPlaceView();
			return View("Edit", bannerplaceView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  bannerplace = Repository.BannerPlaces.FirstOrDefault(p => p.ID == id); 

			if (bannerplace != null) {
				var bannerplaceView = (BannerPlaceView)ModelMapper.Map(bannerplace, typeof(BannerPlace), typeof(BannerPlaceView));
				return View(bannerplaceView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(BannerPlaceView bannerplaceView)
        {
            if (ModelState.IsValid)
            {
                var bannerplace = (BannerPlace)ModelMapper.Map(bannerplaceView, typeof(BannerPlaceView), typeof(BannerPlace));
                if (bannerplace.ID == 0)
                {
                    Repository.CreateBannerPlace(bannerplace);
                }
                else
                {
                    Repository.UpdateBannerPlace(bannerplace);
                }
                return RedirectToAction("Index");
            }
            return View(bannerplaceView);
        }

        public ActionResult Delete(int id)
        {
            var bannerplace = Repository.BannerPlaces.FirstOrDefault(p => p.ID == id);
            if (bannerplace != null)
            {
                    Repository.RemoveBannerPlace(bannerplace.ID);
            }
			return RedirectToAction("Index");
        }
	}
}