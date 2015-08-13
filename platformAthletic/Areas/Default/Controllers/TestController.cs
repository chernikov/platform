using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateData;
using platformAthletic.Helpers;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools.Mail;

namespace platformAthletic.Areas.Default.Controllers
{
    public class TestController : DefaultController
    {
        public ActionResult Index()
        {
            //just one
            var rand = new Random((int)DateTime.Now.Current().Ticks);

            for (int i = 0; i < 5; i++)
            {
                //create coaches 
                var name = Name.GetRandom();
                var surname = Surname.GetRandom();
                var email = Email.GetRandom(name, surname);

                
                var coachView = new RegisterTeamView()
                {
                    FirstName = name,
                    LastName = surname,
                    PaymentType = (int)RegisterUserView.PaymentTypeEnum.CreditCard,
                    PhoneNumber = Phone.GetRandom(),
                    Email = email,
                    Password = "123456",
                    ConfirmPassword = "123456",
                    Team = new TeamView
                    {
                        Name = GenerateData.Team.GetRandom(),
                        StateID = rand.Next() % 50,
                    },
                };

                var coach = (User)ModelMapper.Map(coachView, typeof(RegisterTeamView), typeof(User));

                Repository.CreateUser(coach);

                var userRole = new UserRole()
                {
                    UserID = coach.ID,
                    RoleID = 2 //coach
                };
                Repository.CreateUserRole(userRole);

                //process payment
                //create team
                var team = (Model.Team)ModelMapper.Map(coachView.Team, typeof(TeamView), typeof(Model.Team));
                team.UserID = coach.ID;
                Repository.CreateTeam(team);

                //select start season
                var userSeason = new UserSeason
                {
                    SeasonID = 1,
                    UserID = coach.ID,
                    StartDay = DateTime.Now.Current().AddDays(-10)
                };

                Repository.CreateUserSeason(userSeason);

                //select equpment 
                for (int j = 0; j < 5; j++)
                {
                    var userEquipment = new UserEquipment()
                    {
                        EquipmentID = rand.Next() % 16 + 2,
                        UserID = coach.ID
                    };

                    if (!Repository.UserEquipments.Any(p => p.UserID == userEquipment.UserID && p.EquipmentID == userEquipment.EquipmentID))
                    {
                        Repository.CreateUserEquipment(userEquipment);
                    }
                }

                //create team's players 
                var restoreSetting = new Setting()
                {
                    Name = "CurrentDate",
                    Value = DateTime.Now.Current().Date.ToString()
                };
                var max = (rand.Next() % 20) + 20;

                Repository.SaveSetting(restoreSetting);
                for (int j = 0; j < max; j++)
                {
                    var playerName = Name.GetRandom();
                    var playerSurname = Surname.GetRandom();
                    var playerEmail = Email.GetRandom(playerName, playerSurname);

                    var playerView = new PlayerUserView
                    {
                        FirstName = playerName,
                        LastName = playerSurname,
                        Email = playerEmail,
                        FieldPositionID = (rand.Next() % 8) + 1,
                        PlayerOfTeamID = team.ID,
                    };

                    var player = (User)ModelMapper.Map(playerView, typeof(PlayerUserView), typeof(User));

                    player.Password = "123456";
                    Repository.CreateUser(player);

                    var playerRole = new UserRole()
                    {
                        UserID = player.ID,
                        RoleID = 3 //player
                    };
                    Repository.CreateUserRole(playerRole);

                    GenerateProgressAndAttendance(rand, restoreSetting, player, userSeason, 20);
                }

                //kill each third 
                var players = Repository.Users.Where(p => p.PlayerOfTeamID == team.ID);

                foreach (var player in players.ToList())
                {
                    if (rand.Next() % 4 == 0)
                    {
                        Repository.RemoveUser(player.ID);
                    }
                }

                var halfSetting = new Setting()
                {
                    Name = "CurrentDate",
                    Value = DateTime.Now.Current().AddDays(60).Date.ToString()
                };
                Repository.SaveSetting(halfSetting);
                max = (rand.Next() % 5) + 5;
                for (int j = 0; j < max; j++)
                {
                    var playerName = Name.GetRandom();
                    var playerSurname = Surname.GetRandom();
                    var playerEmail = Email.GetRandom(playerName, playerSurname);

                    var playerView = new PlayerUserView
                    {
                        FirstName = playerName,
                        LastName = playerSurname,
                        Email = playerEmail,
                        FieldPositionID = (rand.Next() % 8) + 1,
                        PlayerOfTeamID = team.ID,
                    };

                    var player = (User)ModelMapper.Map(playerView, typeof(PlayerUserView), typeof(User));

                    player.Password = "123456";
                    Repository.CreateUser(player);

                    var playerRole = new UserRole()
                    {
                        UserID = player.ID,
                        RoleID = 3 //player
                    };
                    Repository.CreateUserRole(userRole);

                    GenerateProgressAndAttendance(rand, halfSetting, player, userSeason, 10);
                }

                Repository.SaveSetting(restoreSetting);


            }
            return null;
        }

        public ActionResult SetPlayers()
        {
            foreach (var user in Repository.Users.ToList())
            {
                if (!user.UserRoles.Any())
                {
                    var userRole = new UserRole()
                    {
                        UserID = user.ID,
                        RoleID = 3 //player
                    };

                    Repository.CreateUserRole(userRole);

                }

            }
            return null;
        }
        private void GenerateProgressAndAttendance(Random rand, Setting restoreSetting, Model.User player, UserSeason userSeason, int weeks)
        {
            DateTime saveDate = DateTime.Parse(restoreSetting.Value);

            var squat = rand.Next(100) + rand.Next(100) + 50;
            var bench = rand.Next(120) + rand.Next(120) + 60;
            var clean = rand.Next(130) + rand.Next(130) + 65;

            Repository.SetSbcValue(player.ID, SBCValue.SbcType.Squat, squat);
            Repository.SetSbcValue(player.ID, SBCValue.SbcType.Bench, bench);
            Repository.SetSbcValue(player.ID, SBCValue.SbcType.Clean, clean);


            for (int k = 0; k < weeks; k++)
            {
                var setting = new Setting()
                {
                    Name = "CurrentDate",
                    Value = saveDate.AddDays(k * 6).Date.ToString()
                };
                Repository.SaveSetting(setting);
                squat = squat + rand.Next(5) + rand.Next(5);
                bench = bench + rand.Next(6) + rand.Next(6);
                clean = clean + rand.Next(6) + rand.Next(7);
                Repository.SetSbcValue(player.ID, SBCValue.SbcType.Squat, squat);
                Repository.SetSbcValue(player.ID, SBCValue.SbcType.Bench, bench);
                Repository.SetSbcValue(player.ID, SBCValue.SbcType.Clean, clean);
            }

            Repository.SaveSetting(restoreSetting);

            for (int k = 0; k < 6 * weeks; k++)
            {
                var setting = new Setting()
                {
                    Name = "CurrentDate",
                    Value = saveDate.AddDays(k).Date.ToString()
                };
                Repository.SaveSetting(setting);
                var attendance = rand.Next() % 3 == 1;
                Repository.SetAttendance(player.ID, attendance, userSeason.ID);
            }
            Repository.SaveSetting(restoreSetting);
        }

        public ActionResult Mail(string email = "chernikov@gmail.com")
        {
            MailSender.SendMail(email, "Welcome to Platform!", "TEST");

            SendWelcomeCoachMail(email, "Welcome to Platform! (coach)", CurrentUser.Email, CurrentUser.Password);

            SendWelcomeIndividualMail(email, "Welcome to Platform! (individual)", CurrentUser.Email, CurrentUser.Password);

            SendWelcomePlayerMail(email, "Welcome to Platform! (athlete)", "Your coach", CurrentUser.Email, CurrentUser.Password);

            SendWelcomeAssistantMail(email, "Welcome to Platform! (assistant)", "Your coach", CurrentUser.Email, CurrentUser.Password);

            SendForgotPasswordMail(email, "Your password for plt4m.com",  CurrentUser.Email, CurrentUser.Password);

            SendResendMail(email, "You email changed successfully! (plt4m.com)", CurrentUser.Email, CurrentUser.Password);

            return Content("OK");
        }
    }
}
