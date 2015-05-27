using AutoMapper;
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
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class SettingController : DefaultController
    {
        protected string DestinationDir = "Media/files/uploads/";

        [HttpGet]
        public ActionResult Index()
        {
            var settingInfoView = (SettingInfoView)Mapper.Map(CurrentUser, typeof(User), typeof(SettingInfoView));
            return View(settingInfoView);
        }

        [HttpPost]
        public ActionResult Index(SettingInfoView settingInfoView)
        {
            if (ModelState.IsValid)
            {
                var user = (User)Mapper.Map(settingInfoView, typeof(SettingInfoView), typeof(User));
                Repository.UpdateUser(user);
                if (CurrentUser.OwnTeam != null)
                {
                    var team = (Team)Mapper.Map(settingInfoView, typeof(SettingInfoView), typeof(Team));
                    team.ID = CurrentUser.OwnTeam.ID;
                    Repository.UpdateSettingTeam(team);
                }
            }
            return View(settingInfoView);
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            var changePasswordView = new ChangePasswordView()
            {
                ID = CurrentUser.ID
            };
            return View(changePasswordView);
        }

        [HttpPost]
        public ActionResult UpdatePassword(ChangePasswordView changePasswordView)
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Password = changePasswordView.NewPassword;
                Repository.ChangePassword(CurrentUser);
                ViewBag.Message = "Changed";
                changePasswordView = new ChangePasswordView
                {
                    ID = CurrentUser.ID
                };
            }
            return View(changePasswordView);
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {

            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=800&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + DestinationDir + uFile });
        }


        public ActionResult CurrentDate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetCurrentDate(DateTime dateTime)
        {
            var setting = new Setting()
            {
                Name = "CurrentDate",
                Value = dateTime.Date.ToString()
            };
            Repository.SaveSetting(setting);

            return Json(new
            {
                result = "ok"
            });
        }

       
    }
}
