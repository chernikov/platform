using ImageResizer;
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

namespace platformAthletic.Areas.Default.Controllers
{
    public class UserController : DefaultController
    {
        protected string DestinationDir = "Media/files/avatars/";
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
                foreach(var item in list) 
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

        public ActionResult ChangeSbc(int id, SBCValue.SbcType type, double value)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null &&  user.CanEditTeamData(CurrentUser))
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
    }
}