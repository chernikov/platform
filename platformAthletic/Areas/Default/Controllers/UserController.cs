﻿using ImageResizer;
using platformAthletic.Global;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;
using ManageAttribute;
using System.Net;
using System.Text.RegularExpressions;
using platformAthletic.Tools.Video;
using platformAthletic.Models.ViewModels;

namespace platformAthletic.Areas.Default.Controllers
{
    public class UserController : DefaultController
    {
        protected string DestinationDir = "Media/files/avatars/";

        protected string DestinationDirVideo = "Media/files/uservideos/";

        public ActionResult Index(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);

            return View(user);
        }

        public ActionResult MyPage()
        {
            return View("Index", CurrentUser);
        }


        public ActionResult UserInfo(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            return View(user);
        }

        public ActionResult SbcData(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            return View(user);
        }
        [HttpGet]
        public ActionResult EditUserInfo(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user == null)
            {
                return null;
            }
            var userInfoView = (UserInfoView)ModelMapper.Map(user, typeof(User), typeof(UserInfoView));
            return View(userInfoView);
        }

        [HttpPost]
        public ActionResult EditUserInfo(UserInfoView userInfoView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(userInfoView, typeof(UserInfoView), typeof(User));

                Repository.UpdateUserInfo(user);

                var list = Repository.UserFieldPositions.Where(p => p.UserID == user.ID).ToList();
                foreach (var item in list)
                {
                    Repository.RemoveUserFieldPosition(item.ID);
                }
                foreach (var userFieldPosition in userInfoView.Sports.Where(p => p.SportID.HasValue))
                {
                    Repository.CreateUserFieldPosition(new UserFieldPosition()
                    {
                        UserID = user.ID,
                        SportID = userFieldPosition.SportID.Value,
                        FieldPositionID = userFieldPosition.FieldPositionID
                    });
                }
            }

            return View(userInfoView);
        }

        public ActionResult SaveUserField(int id, User.FieldType prop, string value)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.SetUserField(id, prop, value);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SchoolRank(int id)
        {
            var user = Repository.TeamPlayersUsers.FirstOrDefault(p => p.ID == id);
            if (user == null)
            {
                return null;
            }
            return View(new SchoolRankInfo(user));
        }

        public ActionResult Rank(int id)
        {
            var user = Repository.TeamPlayersUsers.FirstOrDefault(p => p.ID == id);
            if (user == null)
            {
                return null;
            }
            var rankInfo = new RankInfo(user);
            ViewBag.RankInfo = rankInfo;
            return View(user);
        }

        public ActionResult UserVideo(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user == null)
            {
                return null;
            }
            var list = user.UserVideos.ToList();
            ViewBag.User = user;
            return View(list);
        }

        public ActionResult ChangeSbc(int id, SBCValue.SbcType type, double value)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditTeamData(CurrentUser))
            {
                Repository.ChangeSbcValue(id, type, value);
                var newUser = Repository.Users.FirstOrDefault(p => p.ID == id);
                var newValue = 0;
                switch (type)
                {
                    case SBCValue.SbcType.Squat:
                        newValue = (int)newUser.Squat;
                        break;
                    case SBCValue.SbcType.Bench:
                        newValue = (int)newUser.Bench;
                        break;
                    case SBCValue.SbcType.Clean:
                        newValue = (int)newUser.Clean;
                        break;
                }
                return Json(new
                {
                    result = "ok",
                    value = newValue
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Last12WeekPerformance(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var sunday = DateTime.Now.Current().AddDays(-(int)DateTime.Now.DayOfWeek);
                var currentSunday = sunday.AddDays(-7 * 12);
                var labels = new List<string>();
                var sData = new List<int>();
                var bData = new List<int>();
                var cData = new List<int>();
                var tData = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    var sbc = user.SBCHistory(currentSunday);
                    if (sbc != null)
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                        tData.Add((int)(sbc.Squat + sbc.Bench + sbc.Clean));
                        currentSunday = currentSunday.AddDays(7);
                    }
                };
                var datasets = new List<PerformanceGraphInfo>();
                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Squat",
                    fillColor = "transparent",
                    strokeColor = "#ed4848",
                    pointColor = "#ed4848",
                    pointStrokeColor = "#ed4848",
                    pointHighlightFill = "#ed4848",
                    pointHighlightStroke = "#ed4848",
                    datasetFill = false,
                    data = sData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Bench",
                    fillColor = "transparent",
                    strokeColor = "#3bcb67",
                    pointColor = "#3bcb67",
                    pointStrokeColor = "#3bcb67",
                    pointHighlightFill = "#3bcb67",
                    pointHighlightStroke = "#3bcb67",
                    datasetFill = false,
                    data = bData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Clean",
                    fillColor = "transparent",
                    strokeColor = "#60b1c2",
                    pointColor = "#60b1c2",
                    pointStrokeColor = "#60b1c2",
                    pointHighlightFill = "#60b1c2",
                    pointHighlightStroke = "#60b1c2",
                    datasetFill = false,
                    data = cData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Total",
                    fillColor = "transparent",
                    strokeColor = "#495b6c",
                    pointColor = "#495b6c",
                    pointStrokeColor = "#495b6c",
                    pointHighlightFill = "#495b6c",
                    pointHighlightStroke = "#495b6c",
                    datasetFill = false,
                    data = tData
                });
                var data = new
                {
                    labels,
                    datasets
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }


        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {

            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + DestinationDir + uFile });
        }

        [HttpGet]
        public ActionResult UploadVideo(int id)
        {
            return View(new UserVideoView()
            {
                UserID = id
            });
        }

        [HttpPost]
        public ActionResult UploadVideo(UserVideoView userVideoView)
        {
            if (ModelState.IsValid)
            {
                var userVideo = (UserVideo)ModelMapper.Map(userVideoView, typeof(UserVideoView), typeof(UserVideo));
                userVideo.ID = 0;
                /*userVideo.UserID = CurrentUser.ID;*/
                userVideo.VideoCode = VideoHelper.GetVideoByUrl(userVideo.VideoUrl, 800, 600);
                if (!string.IsNullOrWhiteSpace(userVideo.VideoUrl))
                {
                    var url = VideoHelper.GetVideoThumbByUrl(userVideo.VideoUrl);
                    var webClient = new WebClient();
                    var bytes = webClient.DownloadData(url);
                    var stream = new MemoryStream(bytes);

                    var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                    userVideo.Preview = "/" + Path.Combine(DestinationDirVideo, uFile);
                    var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDirVideo), uFile);

                    ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));

                    Repository.CreateUserVideo(userVideo);
                    return View("_OK");
                }
                else
                {
                    ModelState.AddModelError("VideoUrl", "Can't parse this link");
                }
                
            }
            return View("UploadVideoBody", userVideoView);
        }


        [TempAction]
        public ActionResult GenerateUserVideos()
        {
            var regexTemplate = ".*http://www\\.youtube\\.com/watch\\?v=(?<code>.*?)\" target=\"_blank\">";

            var listOfCodes = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                var client = new WebClient();

                var httpPage = client.DownloadString("http://randomyoutube.net/");

                var regex = new Regex(regexTemplate);
                var matches = regex.Match(httpPage);
                if (matches.Success)
                {
                    var code = matches.Groups["code"].Value;
                    listOfCodes.Add(code);
                }
            }
            var counter = 0;
            var random = new Random((int)DateTime.Now.Ticks);
            var listOfVideos = new List<UserVideo>();
            foreach (var code in listOfCodes)
            {
                var videoUrl = "http://www.youtube.com/watch?v=" + code;


                var videoCode = VideoHelper.GetVideoByUrl(videoUrl, 800, 600);
                try
                {
                    var url = VideoHelper.GetVideoThumbByUrl(videoUrl);
                    var webClient = new WebClient();
                    var bytes = webClient.DownloadData(url);
                    var stream = new MemoryStream(bytes);

                    var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                    var preview = "/" + Path.Combine(DestinationDirVideo, uFile);
                    var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDirVideo), uFile);

                    ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                    var userVideo = new UserVideo()
                    {
                        UserID = 0,
                        Preview = preview,
                        VideoUrl = videoUrl,
                        VideoCode = videoCode,
                        Header = "",
                    };

                    listOfVideos.Add(userVideo);
                }
                catch (Exception ex)
                {
                }
            }

            foreach (var user in Repository.TeamPlayersUsers.Where(p => !p.UserVideos.Any()))
            {
                counter++;
                var count = random.Next(5) + 1;
                for (int i = 0; i < count; i++)
                {
                    var video = listOfVideos.OrderBy(p => Guid.NewGuid()).First();

                    var userVideo = new UserVideo()
                    {
                        UserID = user.ID,
                        Preview = video.Preview,
                        VideoUrl = video.VideoUrl,
                        VideoCode = video.VideoCode,
                        Header = GenerateData.Name.GetRandom() + " " + GenerateData.Team.GetRandom()
                    };
                    Repository.CreateUserVideo(userVideo);
                }
            }

            return Content("OK");
        }
    }
}