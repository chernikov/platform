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
    public class VideoController : AdminController
    {
        protected string DestinationDir = "Media/files/previews/";

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
                video.Preview = GetPreview(video.VideoUrl);
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