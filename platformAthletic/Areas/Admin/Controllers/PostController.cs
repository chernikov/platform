using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using AutoMapper;
using platformAthletic.Tools.Video;
using System.Net;
using System.IO;
using platformAthletic.Tools;
using ImageResizer;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class PostController : AdminController
    {
        protected string DestinationDir = "Media/files/postimages/";

        public ActionResult Index(int page = 1)
        {
            var list = Repository.Posts.OrderByDescending(p => p.ID);
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
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);

            if (post != null)
            {
                var postView = (PostView)ModelMapper.Map(post, typeof(Post), typeof(PostView));
                return View(postView);
            }
            return RedirectToNotFoundPage;
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(PostView postView)
        {
            var existPost = Repository.Posts.FirstOrDefault(p => p.ID == postView.ID);
            if (existPost == null || (existPost.UserID == CurrentUser.ID || CurrentUser.InRoles("admin")))
            {
                if (ModelState.IsValid)
                {
                    var post = (Post)Mapper.Map(postView, typeof(PostView), typeof(Post));
                    post.UserID = CurrentUser.ID;

                    if (!string.IsNullOrWhiteSpace(postView.VideoUrl))
                    {
                        var videoCode = VideoHelper.GetVideoByUrl(postView.VideoUrl, 800, 600);
                        var url = VideoHelper.GetVideoThumbByUrl(postView.VideoUrl);
                        var webClient = new WebClient();
                        var bytes = webClient.DownloadData(url);
                        var stream = new MemoryStream(bytes);
                        var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                        var preview = "/" + Path.Combine(DestinationDir, uFile);
                        var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);

                        ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                        post.VideoCode = videoCode;
                        post.VideoPreview = preview;
                    }
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

        [HttpGet]
        public ActionResult Promote(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);

            if (post != null)
            {
                Repository.PromotePost(post.ID);
            }
            return RedirectToAction("Index");
        }
    }
}