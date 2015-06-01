using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;
namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles="coach,individual,player")]
    public class TutorialController : DefaultController
    {

        //
        // GET: /Default/Tutorial/

        public ActionResult Index()
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Tutorial) 
            {
                var name = string.Empty;
                if (CurrentUser.InRoles("coach"))
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
                if (step <= 0)
                {
                    Repository.StepTutorial(CurrentUser.ID, 1);
                    step = 1;
                }
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
            return Json(new {result = "ok"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EndTutorial()
        {
           // if (CurrentUser.Mode == (int)Model.User.ModeEnum.Tutorial)
            {
                if (CurrentUser.InRoles("coach"))
                {
                    Repository.StartTestMode(CurrentUser.ID);
                    GeneratePhantoms();
                }
                else
                {
                    Repository.StartTodoMode(CurrentUser.ID);
                }
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        private void GeneratePhantoms()
        {
            var rand = new Random((int)DateTime.Now.Current().Ticks);
            var group = new Group()
            {
                IsPhantom = true,
                Name = "Football",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("John", "Alexander", rand, group);
            CreateUser("Tim", "Brown", rand, group);
            CreateUser("Sarah", "Dodd", rand, group);
            CreateUser("Bill", "Bean", rand, group);
            group = new Group()
            {
                IsPhantom = true,
                Name = "Women's Lax",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Annie", "Faust", rand, group);
            CreateUser("Michelle", "Jenkins", rand, group);
            CreateUser("Sarah", "Parker", rand, group);
            CreateUser("Amy", "Shwartz", rand, group);
            CreateUser("Christy", "Hanson", rand, group);
           
            group = new Group()
            {
                IsPhantom = true,
                Name = "Mens Bball",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Bill", "Gallihugh", rand, group);
            CreateUser("Jason", "Hinton", rand, group);
            CreateUser("Scott", "King", rand, group);
            CreateUser("Jared", "Nepa", rand, group);
            CreateUser("Frank", "Young", rand, group);


            group = new Group()
            {
                IsPhantom = true,
                Name = "Field Hockey",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Amanda", "Gilmore", rand, group);
            CreateUser("Laura", "Stein", rand, group);
            CreateUser("Jess", "Smith", rand, group);
            CreateUser("Carla", "Parker", rand, group);
            CreateUser("Caitlin", "Beber", rand, group);
        }

        private void CreateUser(string firstName, string lastName, Random rand, Group group)
        {

            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = firstName.ToLower()+"."+lastName.ToLower()+"@hotmail.com",
                PlayerOfTeamID = CurrentUser.OwnTeam.ID,
                Password = "123456",
                GroupID = group.ID
            };

            Repository.CreateUser(user);

            var playerRole = new UserRole()
            {
                UserID = user.ID,
                RoleID = 3 //player
            };
            Repository.CreateUserRole(playerRole);
            for (int i = 0; i < 3; i++)
            {
                var fieldPosition = Repository.FieldPositions.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                var userPosition = new UserFieldPosition()
                {
                    UserID = user.ID,
                    FieldPositionID = fieldPosition.ID,
                    SportID = fieldPosition.Sport.ID
                };
                Repository.CreateUserFieldPosition(userPosition);
            }
            var squat = 200 + rand.Next(20) * 5;
            var bench = 150 + rand.Next(10) * 5;
            var clean = 100 + rand.Next(7) * 5;
            var startDate = DateTime.Now.Current().AddDays(-90);
            for (int i = 0; i < 3; i++)
            {
                Repository.SetSbcValue(user.ID, SBCValue.SbcType.Squat, squat, startDate);
                Repository.SetSbcValue(user.ID, SBCValue.SbcType.Bench, bench, startDate);
                Repository.SetSbcValue(user.ID, SBCValue.SbcType.Clean, clean, startDate);
                squat += rand.Next(10) * 5;
                bench += rand.Next(5) * 5;
                clean += rand.Next(3) * 5;
                startDate = startDate.AddDays(21);
            }

            startDate = DateTime.Now.Current().AddDays(-90);
            while (startDate < DateTime.Now)
            {
                startDate = startDate.AddDays(rand.Next(3)+2);
                var userAttendance = new UserAttendance()
                {
                    UserID = user.ID,
                    AddedDate = startDate,
                    UserSeasonID = user.CurrentSeason.ID
                };
                Repository.CreateUserAttendance(userAttendance);
            }
        }
    }
}
