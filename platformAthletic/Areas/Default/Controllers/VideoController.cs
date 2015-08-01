using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Tools.Video;
using System.Net;
using System.IO;
using platformAthletic.Tools;
using ImageResizer;

namespace platformAthletic.Areas.Default.Controllers
{
    public class VideoController : DefaultController
    {
        protected string DestinationDir = "Media/files/previews/";

        public ActionResult Index(int sortType = 1, string searchString = null)
        {
            var videoCollection = new VideoCollection((VideoCollection.SortTypeEnum)sortType, searchString);
            return View(videoCollection);
        }

        public ActionResult Video(int id)
        {
            var video = Repository.Videos.FirstOrDefault(p => p.ID == id);
            if (video != null)
            {
                return View(video);
            }
            return null;
        }

        public ActionResult PillarVideo(int id)
        {
            var pillar = Repository.PillarTypes.FirstOrDefault(p => p.ID == id);
            if (pillar != null)
            {
                return View(pillar);
            }
            return null;
        }

        public ActionResult JsonVideos()
        {
            var videos = Repository.Videos.ToList();

            return Json(new
            {
                data = videos.Select(p => new
                {
                    id = p.ID,
                    sortType = 1,
                    header = p.Header,
                    preview = p.Preview
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JsonPillars()
        {
            var pillarTypes = Repository.PillarTypes.ToList();

            return Json(new
            {
                data = pillarTypes.Select(p => new
                {
                    id = p.ID,
                    sortType = 2,
                    header = p.Name,
                    preview = p.Preview
                })
            }, JsonRequestBehavior.AllowGet);
        }

        //TODO:TempAction
        public ActionResult SetCode()
        {
            var videos = Repository.Videos.ToList();
            foreach (var video in videos)
            {
                var code = VideoHelper.GetVideoByUrl(video.VideoUrl, 800, 600);
                video.VideoCode = code;
                Repository.UpdateVideo(video);
            }

            var pillars = Repository.PillarTypes.ToList();
            foreach (var pillar in pillars)
            {
                var code = VideoHelper.GetVideoByUrl(pillar.VideoUrl, 800, 600);
                pillar.VideoCode = code;
                Repository.UpdatePillarType(pillar);
            }
            return null;
        }

        //TODO:TempAction
        public ActionResult SetPreview()
        {
            var videos = Repository.Videos.Where(p => p.Preview == "" || p.Preview == null).ToList();
            foreach (var video in videos)
            {
                var url = VideoHelper.GetVideoThumbByUrl(video.VideoUrl);
                var webClient = new WebClient();
                var bytes = webClient.DownloadData(url);
                var stream = new MemoryStream(bytes);

                var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                var urlFile = "/" + Path.Combine(DestinationDir, uFile);
                var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);

                ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                video.Preview = urlFile;
                Repository.UpdateVideo(video);
            }

            var pillars = Repository.PillarTypes.ToList();
            foreach (var pillar in pillars)
            {
                var url = VideoHelper.GetVideoThumbByUrl(pillar.VideoUrl);
                var webClient = new WebClient();
                var bytes = webClient.DownloadData(url);
                var stream = new MemoryStream(bytes);

                var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                var urlFile = "/" + Path.Combine(DestinationDir, uFile);
                var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);

                ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));

                pillar.Preview = urlFile;
                Repository.UpdatePillarType(pillar);
            }
            return null;
        }
    }
}
