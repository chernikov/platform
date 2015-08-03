using platformAthletic.Model;
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
    public class ManageAssistantController : DefaultController
    {
        public ActionResult Index()
        {
            var list = CurrentUser.OwnTeam.Assistants.Where(p => !p.IsDeleted).OrderBy(p => p.LastName).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit",new AssistantUserView()
            {
                AssistantOfTeamID = CurrentUser.OwnTeam.ID
            });
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditSBC(CurrentUser))
            {
                var assistantUserView = (AssistantUserView)ModelMapper.Map(user, typeof(User), typeof(AssistantUserView));
                return View(assistantUserView);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Edit(AssistantUserView assistantUserView)
        {
            if (assistantUserView.Password == "" && assistantUserView.ID != 0)
            {
                ModelState.AddModelError("Password", "Enter Password");
            }
            if (ModelState.IsValid)
            {
                
                var user = (User)ModelMapper.Map(assistantUserView, typeof(AssistantUserView), typeof(User));
                if (user.ID == 0)
                {
                    user.AssistantOfTeamID = CurrentUser.OwnTeam.ID;
                    user.Password = StringExtension.CreateRandomPassword(6, "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                    Repository.CreateUser(user);
                    var userRole = new UserRole()
                    {
                        UserID = user.ID,
                        RoleID = 5 //assistant
                    };
                    Repository.CreateUserRole(userRole);

                    NotifyMail.SendNotify(Config, "RegisterAssistant", user.Email,
                        (u, format) => format,
                        (u, format) => string.Format(format, CurrentUser.FirstName + " " + CurrentUser.LastName, u.Email, u.Password),
                        user);
                    //Repository.StartTutorial(user.ID);
                }
                else
                {
                    Repository.UpdateAssistant(user);
                }
                return View("_OK");

            }
            return View("EditBody", assistantUserView);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null && user.CanEditSBC(CurrentUser))
            {
                var assistantUserView = (AssistantUserView)ModelMapper.Map(user, typeof(User), typeof(AssistantUserView));
                return View(assistantUserView);
            }
            return View("_OK");
        }


        [HttpGet]
        public ActionResult SendActivation(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                NotifyMail.SendNotify(Config, "RegisterAssistant", user.Email,
                      (u, format) => format,
                      (u, format) => string.Format(format, CurrentUser.FirstName + " " + CurrentUser.LastName, u.Email, u.Password),
                      user);
                Repository.ResendRegister(user);
                return View(user);
            }
            return View("_OK");
        }

        [HttpPost]
        public ActionResult Delete(AssistantUserView assistantUserView)
        {
            var id = assistantUserView.ID;
            Repository.RemoveUser(id);
            return View("_OK");
        }
    }
}
