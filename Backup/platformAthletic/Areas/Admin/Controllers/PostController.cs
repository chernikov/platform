using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class PostController : AdminController
    {
		public ActionResult Index(int page = 1)
        {
			var list = Repository.Posts;
			var data = new PageableData<Post>();
			data.Init(list, page, "Index");
			return View(data);
		}

		public ActionResult Create() 
		{
            var postView = new PostView()
            {
                UserID = CurrentUser.ID
            };
			return View("Edit", postView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  post = Repository.Posts.FirstOrDefault(p => p.ID == id); 

			if (post != null) 
            {
				var postView = (PostView)ModelMapper.Map(post, typeof(Post), typeof(PostView));
				return View(postView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
        [ValidateInput(false)]
		public ActionResult Edit(PostView postView)
        {
            if (ModelState.IsValid)
            {
                var post = (Post)ModelMapper.Map(postView, typeof(PostView), typeof(Post));
                if (post.ID == 0)
                {
                    Repository.CreatePost(post);
                }
                else
                {
                    Repository.UpdatePost(post);
                }
                return RedirectToAction("Index");
            }
            return View(postView);
        }

        public ActionResult Delete(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);
            if (post != null)
            {
                    Repository.RemovePost(post.ID);
            }
			return RedirectToAction("Index");
        }
	}
}