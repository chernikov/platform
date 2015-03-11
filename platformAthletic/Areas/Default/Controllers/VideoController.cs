using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;

namespace platformAthletic.Areas.Default.Controllers
{
    public class VideoController : DefaultController
    {
        public ActionResult Index(bool pillar, int? id = null)
        {
            
            if (pillar)
            {
                PillarType video = null;
                if (id.HasValue)
                {
                    video = Repository.PillarTypes.FirstOrDefault(p => p.ID == id.Value);
                }
                if (video == null)
                {
                    video = Repository.PillarTypes.FirstOrDefault();
                }

                return View("IndexPillarVideo", video);
            }
            else
            {
                Video video = null;
                if (id.HasValue)
                {
                    video = Repository.Videos.FirstOrDefault(p => p.ID == id.Value);
                }
                if (video == null)
                {
                    video = Repository.Videos.FirstOrDefault();
                }
                if (video != null)
                {
                    return View(video);
                }
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult VideoMenu(int SortType, string SearchString = null)
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
                        TrainingName = p.Training.Name ?? p.Header
                    }).ToList();
                    break;

                case VideoCollection.SortTypeEnum.Pillar:
                    collection.Videos = Repository.PillarTypes.Select(p => new VideoCollection.InnerVideo()
                    {
                        ID = p.ID,
                        Header = p.Name,
                        VideoCode = p.VideoCode,
                        VideoUrl = p.VideoUrl,
                        TrainingName = p.Name
                    }).ToList();
                    break;
            }
            return View(collection);
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
    }
}
