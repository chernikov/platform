using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;
using platformAthletic.Models.Info;
namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles="coach,assistant,individual,player")]
    public class TutorialController : DefaultController
    {
        public ActionResult Index()
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Tutorial) 
            {
                var name = string.Empty;
                if (CurrentUser.InRoles("coach,assistant"))
                {
                    name = "Coach_";
                }
                if (CurrentUser.InRoles("individual"))
                {
                    name = "Individual_";
                }
                if (CurrentUser.InRoles("player"))
                {
                    name = "Player_";
                }
                var step = CurrentUser.TutorialStep;
                return View(name + step.ToString("D2"));
            }
            return null;
        }

        public ActionResult Step(int id)
        {
          
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Tutorial)
            {
                Repository.StepTutorial(CurrentUser.ID, id);
            }
            return Json(new {result = "ok", id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EndTutorial()
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Tutorial)
            {
                if (CurrentUser.InRoles("coach,assistant"))
                {
                    if (CurrentUser.OwnTeam != null && CurrentUser.OwnTeam.Players.Any(p => !p.IsPhantom && !p.IsDeleted))
                    {
                        Repository.StartTodoMode(CurrentUser.ID);
                        if (CurrentUser.OwnTeam != null && CurrentUser.OwnTeam.Players.Any(p => p.IsPhantom))
                        {
                            Repository.RemovePhantoms(CurrentUser.ID);
                        }
                    }
                    else
                    {
                        Repository.StartTestMode(CurrentUser.ID);
                    }
                }
                else
                {
                    Repository.StartTodoMode(CurrentUser.ID);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EndTest()
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Test)
            {
                Repository.StartTodoMode(CurrentUser.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Todo(int? id = null)
        {
            
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                if (id.HasValue)
                {
                    Repository.SetTodo(CurrentUser.ID, (Model.User.TodoEnum)id.Value);
                }
                if (CurrentUser.InRoles("coach,assistant"))
                {
                    var todo = new CoachTodoListInfo(CurrentUser.Todo);
                    return View("CoachTodo", todo);
                }
                if (CurrentUser.InRoles("individual"))
                {
                    var todo = new IndividualTodoListInfo(CurrentUser.Todo);
                    return View("IndividualTodo", todo);
                }
                if (CurrentUser.InRoles("player"))
                {
                    var todo = new PlayerTodoListInfo(CurrentUser.Todo);
                    return View("PlayerTodo", todo);
                }
            }
            return null;
        }

        public ActionResult TodoSnippet(int id)
        {
            var name = "";
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                if (CurrentUser.InRoles("coach,assistant"))
                {
                    name = "TodoCoach_";
                }
                if (CurrentUser.InRoles("individual"))
                {
                    name = "TodoIndividual_";
                }
                if (CurrentUser.InRoles("player"))
                {
                    name = "TodoPlayer_";
                }

                return View(name + id.ToString("D2"));
            }
            return null;
        }

        public ActionResult Info(string message, string button = "Ok")
        {
            ViewBag.Message = message;
            ViewBag.Button = button;
            return View();
        }

        public ActionResult ShowStopTestMode()
        {
            return View();
        }

        public ActionResult StopTestMode()
        {
            Repository.RemovePhantoms(CurrentUser.ID);
            return Redirect("/dashboard");
        }
    
        public ActionResult StartTrainingDate() 
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                try
                {
                    Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
                } catch {}
            }
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }
          
            

       
    }
}
