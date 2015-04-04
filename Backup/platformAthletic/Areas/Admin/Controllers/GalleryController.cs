using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class GalleryController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Galleries.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var galleryView = new GalleryView();
			return View("Edit", galleryView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  gallery = Repository.Galleries.FirstOrDefault(p => p.ID == id); 

			if (gallery != null) {
				var galleryView = (GalleryView)ModelMapper.Map(gallery, typeof(Gallery), typeof(GalleryView));
				return View(galleryView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(GalleryView galleryView)
        {
            if (ModelState.IsValid)
            {
                var gallery = (Gallery)ModelMapper.Map(galleryView, typeof(GalleryView), typeof(Gallery));
                if (gallery.ID == 0)
                {
                    Repository.CreateGallery(gallery);
                }
                else
                {
                    Repository.UpdateGallery(gallery);
                }
                return RedirectToAction("Index");
            }
            return View(galleryView);
        }

        public ActionResult Delete(int id)
        {
            var gallery = Repository.Galleries.FirstOrDefault(p => p.ID == id);
            if (gallery != null)
            {
                    Repository.RemoveGallery(gallery.ID);
            }
			return RedirectToAction("Index");
        }
	}
}