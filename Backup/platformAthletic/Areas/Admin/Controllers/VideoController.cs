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
    public class VideoController : AdminController
    {

        public ActionResult Index(int page = 1)
        {
            var list = Repository.Videos;
            var data = new PageableData<Video>();
            data.Init(list, page, "Index");
            return View(data);
        }

        public ActionResult Create()
        {
            var videoView = new VideoView();
            return View("Edit", videoView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);

            if (video != null)
            {
                var videoView = (VideoView)ModelMapper.Map(video, typeof(Video), typeof(VideoView));
                return View(videoView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(VideoView videoView)
        {
            if (ModelState.IsValid)
            {
                var video = (Video)ModelMapper.Map(videoView, typeof(VideoView), typeof(Video));
                video.VideoCode = VideoHelper.GetVideoByUrl(video.VideoUrl);

                if (video.ID == 0)
                {
                    Repository.CreateVideo(video);
                }
                else
                {
                    Repository.UpdateVideo(video);
                }
                return RedirectToAction("Index");
            }
            return View(videoView);
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
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {
                Repository.RemoveVideo(video.ID);
            }
            return RedirectToAction("Index");
        }
    }
}