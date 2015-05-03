using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Tools.Video;

namespace platformAthletic.Areas.Default.Controllers
{
    public class VideoController : DefaultController
    {
        public ActionResult Index(int SortType = 1, string SearchString = null)
        {
            var collection = new VideoCollection()
            {
                SortType = (VideoCollection.SortTypeEnum)SortType,
                SearchString = SearchString,

            };

            switch ((VideoCollection.SortTypeEnum)SortType)
            {
                case VideoCollection.SortTypeEnum.Training:
                    collection.Videos = Repository.Videos.Select(p => new VideoCollection.InnerVideo()
                    {
                        ID = p.ID,
                        Header = p.Training.Name ?? p.Header,
                        VideoCode = p.VideoCode,
                        VideoUrl = p.VideoUrl,
                        TrainingName = p.Training.Name ?? p.Header,
                        Preview = p.Preview,
                    }).ToList();
                    break;

                case VideoCollection.SortTypeEnum.Pillar:
                    collection.Videos = Repository.PillarTypes.Select(p => new VideoCollection.InnerVideo()
                    {
                        ID = p.ID,
                        Header = p.Name,
                        VideoCode = p.VideoCode,
                        VideoUrl = p.VideoUrl,
                        TrainingName = p.Name,
                        Preview = p.Preview,
                    }).ToList();
                    break;
            }
            return View(collection);
        }

        public ActionResult SetPreview()
        {
            var videos = Repository.Videos.ToList();
            foreach (var video in videos)
            {
                video.Preview = VideoHelper.GetVideoThumbByUrl(video.VideoUrl);
                Repository.UpdateVideo(video);
            }

            var pillars = Repository.PillarTypes.ToList();
            foreach (var pillar in pillars)
            {
                pillar.Preview = VideoHelper.GetVideoThumbByUrl(pillar.VideoUrl);
                Repository.UpdatePillarType(pillar);
            }
            return null;
        }
        /*
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
        }*/
    }
}
