﻿using platformAthletic.Model;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools;
using platformAthletic.Tools.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach, assistant")]
    public class ManagePlayerController : DefaultController
    {
        public ActionResult Index()
        {
            var list = CurrentUser.OwnTeam.Players.Where(p => !p.IsDeleted).OrderBy(p => p.LastName).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create(bool testMode=false)
        {
            if (CurrentUser.InTestMode && !testMode)
            {
                return View("TestModeMessage");
            }
            return View("Edit", new PlayerUserView());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditSBC(CurrentUser))
            {
                var playerUserView = (PlayerUserView)ModelMapper.Map(user, typeof(User), typeof(PlayerUserView));
                return View(playerUserView);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Edit(PlayerUserView playerUserView)
        {
            if (ModelState.IsValid)
            {
                if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
                {
                    Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.AddPlayers);
                }

                var user = (User)ModelMapper.Map(playerUserView, typeof(PlayerUserView), typeof(User));
                if (user.ID == 0)
                {
                    user.Password = StringExtension.CreateRandomPassword(8, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                    user.PlayerOfTeamID = CurrentUser.OwnTeam.ID;
                    user.TutorialStep = 1;
                    user.Mode = (int)Model.User.ModeEnum.Tutorial;
                    user.IsPhantom = (User.ModeEnum)CurrentUser.Mode == Model.User.ModeEnum.Test;
                    Repository.CreateUser(user);
                    var userRole = new UserRole()
                    {
                        UserID = user.ID,
                        RoleID = 3 //player
                    };
                    Repository.CreateUserRole(userRole);

                    SendWelcomePlayerMail(user.Email, "Welcome to Platform!", CurrentUser.FirstName + " " + CurrentUser.LastName, user.Email, user.Password);

                    var existFailMail = Repository.FailedMails.FirstOrDefault(p => string.Compare(p.FailEmail, user.Email, true) == 0);
                    if (existFailMail != null)
                    {
                        Repository.RemoveFailedMail(existFailMail.ID);
                    }
                }
                else
                {
                    Repository.UpdateManageUser(user);
                }
                return View("_OK");

            }
            return View(playerUserView);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditSBC(CurrentUser))
            {
                var playerUserView = (PlayerUserView)ModelMapper.Map(user, typeof(User), typeof(PlayerUserView));
                return View(playerUserView);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Delete(PlayerUserView playerUserView)
        {
            var id = playerUserView.ID;
            Repository.RemoveUser(id);
            return View("_OK");
        }

        [HttpGet]
        public ActionResult SendActivation(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                SendWelcomePlayerMail(user.Email, "Welcome to Platform!", CurrentUser.FirstName + " " + CurrentUser.LastName, user.Email, user.Password);
                Repository.ResendRegister(user);
                return View(user);
            }
            return View("_OK");
        }
    }
}
