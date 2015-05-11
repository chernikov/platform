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
using ManageAttribute;

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

        [TempAction]
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
        [TempAction]
        public ActionResult SetPreview()
        {
            var videos = Repository.Videos.ToList();
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

        [TempAction]
        public ActionResult SetLorem()
        {
            var videos = Repository.Videos.ToList();
            foreach (var video in videos)
            {
                video.Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel ultricies enim. In hac habitasse platea dictumst. In erat dui, sagittis id ligula et, tincidunt dapibus turpis. Fusce ullamcorper quam eu felis porttitor tempus. Suspendisse gravida lacus id tincidunt faucibus. Nullam finibus bibendum est, at sollicitudin nulla dapibus eu. In hendrerit ex sit amet lobortis dictum. Phasellus placerat lacus eget magna cursus, sit amet tincidunt magna rhoncus. Sed aliquam lacinia dignissim. Integer porttitor tempor tortor, hendrerit porttitor erat venenatis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit.

Cras imperdiet posuere erat, ac egestas nulla. Pellentesque congue at odio a lacinia. Nulla fringilla lorem eget ullamcorper rhoncus. Nulla sollicitudin tellus libero, vitae pellentesque dui pharetra eget. Cras sit amet suscipit felis, id faucibus ex. Proin non condimentum arcu. Praesent laoreet lorem ac velit bibendum, et commodo nisl luctus. Vivamus facilisis purus id est commodo, ac blandit lectus facilisis. Nunc volutpat viverra ultrices. Quisque cursus vehicula justo, porta imperdiet nisl elementum sit amet. Suspendisse potenti. Vestibulum posuere rhoncus nunc non rutrum. Donec euismod ac tortor et accumsan.

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi laoreet, velit sed dictum feugiat, dui est tempus massa, nec ullamcorper diam tellus at mi. Morbi ut gravida nulla, porttitor sollicitudin tellus. Sed feugiat purus sed rhoncus varius. Praesent luctus libero non dui molestie, non luctus turpis dictum. Donec mattis, augue id consectetur viverra, leo ligula rhoncus enim, id ultrices felis magna in magna. Vivamus luctus dolor et ante sollicitudin lobortis. Mauris eu arcu in sapien vestibulum gravida. Nulla et velit nisl. Donec id metus vitae erat volutpat tincidunt. Vivamus aliquet est velit, sed imperdiet leo eleifend in. Curabitur auctor tempor lorem, id sodales risus porttitor quis. Mauris malesuada vitae purus ac euismod. Maecenas vel orci in velit imperdiet aliquam quis vel lectus. Maecenas pellentesque turpis eu imperdiet ornare.

Ut sed augue blandit, auctor risus quis, ultrices massa. Integer consectetur nisi nec nisi placerat consequat. Donec ultrices nisl ut augue cursus ullamcorper. Donec mattis tempor rhoncus. In non venenatis augue. Cras lorem tellus, interdum eu tincidunt sit amet, ullamcorper a magna. Pellentesque at elementum risus, sed pretium diam. Vivamus tincidunt aliquam lectus, sed accumsan enim. Nullam malesuada vel tortor nec imperdiet. Maecenas metus enim, aliquet eu velit id, accumsan faucibus magna.

Pellentesque eget imperdiet felis. Nulla hendrerit magna vel felis pharetra, non maximus metus condimentum. Fusce vel auctor purus. Aenean placerat nibh nibh, in efficitur dui maximus a. Vivamus imperdiet nisi nunc, sit amet venenatis ligula placerat nec. Morbi congue varius ante, id fringilla elit imperdiet vitae. Nulla a lectus tincidunt, dapibus libero ut, pharetra enim. Etiam placerat dolor metus, sit amet blandit mauris posuere a.";

                Repository.UpdateVideo(video);
            }

            var pillars = Repository.PillarTypes.ToList();
            foreach (var pillar in pillars)
            {
                pillar.Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel ultricies enim. In hac habitasse platea dictumst. In erat dui, sagittis id ligula et, tincidunt dapibus turpis. Fusce ullamcorper quam eu felis porttitor tempus. Suspendisse gravida lacus id tincidunt faucibus. Nullam finibus bibendum est, at sollicitudin nulla dapibus eu. In hendrerit ex sit amet lobortis dictum. Phasellus placerat lacus eget magna cursus, sit amet tincidunt magna rhoncus. Sed aliquam lacinia dignissim. Integer porttitor tempor tortor, hendrerit porttitor erat venenatis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit.

Cras imperdiet posuere erat, ac egestas nulla. Pellentesque congue at odio a lacinia. Nulla fringilla lorem eget ullamcorper rhoncus. Nulla sollicitudin tellus libero, vitae pellentesque dui pharetra eget. Cras sit amet suscipit felis, id faucibus ex. Proin non condimentum arcu. Praesent laoreet lorem ac velit bibendum, et commodo nisl luctus. Vivamus facilisis purus id est commodo, ac blandit lectus facilisis. Nunc volutpat viverra ultrices. Quisque cursus vehicula justo, porta imperdiet nisl elementum sit amet. Suspendisse potenti. Vestibulum posuere rhoncus nunc non rutrum. Donec euismod ac tortor et accumsan.

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi laoreet, velit sed dictum feugiat, dui est tempus massa, nec ullamcorper diam tellus at mi. Morbi ut gravida nulla, porttitor sollicitudin tellus. Sed feugiat purus sed rhoncus varius. Praesent luctus libero non dui molestie, non luctus turpis dictum. Donec mattis, augue id consectetur viverra, leo ligula rhoncus enim, id ultrices felis magna in magna. Vivamus luctus dolor et ante sollicitudin lobortis. Mauris eu arcu in sapien vestibulum gravida. Nulla et velit nisl. Donec id metus vitae erat volutpat tincidunt. Vivamus aliquet est velit, sed imperdiet leo eleifend in. Curabitur auctor tempor lorem, id sodales risus porttitor quis. Mauris malesuada vitae purus ac euismod. Maecenas vel orci in velit imperdiet aliquam quis vel lectus. Maecenas pellentesque turpis eu imperdiet ornare.

Ut sed augue blandit, auctor risus quis, ultrices massa. Integer consectetur nisi nec nisi placerat consequat. Donec ultrices nisl ut augue cursus ullamcorper. Donec mattis tempor rhoncus. In non venenatis augue. Cras lorem tellus, interdum eu tincidunt sit amet, ullamcorper a magna. Pellentesque at elementum risus, sed pretium diam. Vivamus tincidunt aliquam lectus, sed accumsan enim. Nullam malesuada vel tortor nec imperdiet. Maecenas metus enim, aliquet eu velit id, accumsan faucibus magna.

Pellentesque eget imperdiet felis. Nulla hendrerit magna vel felis pharetra, non maximus metus condimentum. Fusce vel auctor purus. Aenean placerat nibh nibh, in efficitur dui maximus a. Vivamus imperdiet nisi nunc, sit amet venenatis ligula placerat nec. Morbi congue varius ante, id fringilla elit imperdiet vitae. Nulla a lectus tincidunt, dapibus libero ut, pharetra enim. Etiam placerat dolor metus, sit amet blandit mauris posuere a.";

                Repository.UpdatePillarType(pillar);
            }
            return null;
        }




    }
}
