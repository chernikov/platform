using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.Info;
using platformAthletic.Tools.Video;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class YoutubeController : AdminController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var youtubeInfoView = new YoutubeInfo()
            {
                VideoUrl = Repository.Settings.First(p => p.Name == "VideoUrl").Value,
                VideoCode = Repository.Settings.First(p => p.Name == "VideoCode").Value,
            };

            return View(youtubeInfoView);
        }

        [HttpPost]
        public ActionResult Index(YoutubeInfo youtubeInfoView)
        {
            youtubeInfoView.VideoCode = VideoHelper.GetVideoByUrl(youtubeInfoView.VideoUrl, 638, 384);
            if (string.IsNullOrEmpty(youtubeInfoView.VideoCode))
            {
                ModelState.AddModelError("VideoUrl", "Enter a correct youtube url. I can't parse");
            }
            if (ModelState.IsValid)
            {
                var videoUrl = Repository.Settings.First(p => p.Name == "VideoUrl");
                videoUrl.Value = youtubeInfoView.VideoUrl.ToString();
                Repository.SaveSetting(videoUrl);

                var videoCode = Repository.Settings.First(p => p.Name == "VideoCode");
                videoCode.Value = youtubeInfoView.VideoCode.ToString();
                Repository.SaveSetting(videoCode);

                ViewBag.Message = "Saved";
            }
            return View(youtubeInfoView);
        }
    }
}
