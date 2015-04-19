using GenerateData;
using Ninject;
using NUnit.Framework;
using platformAthletic.IntegrationTest.Tools;
using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace platformAthletic.IntegrationTest.Test
{

    [TestFixture]
    public class GenerateUserTest
    {
        private Random rand = new Random((int)DateTime.Now.Ticks);

        [Test]
        public void GenerateMoreThanOne()
        {
            for (int i = 0; i < 200; i++)
            {
                try
                {
                    GenerateTeam();
                }
                catch
                {

                }
            }
            for (int i = 0; i < 200; i++)
            {
                try
                {
                    GenerateIndividual();
                }
                catch
                {

                }
            }
        }

        private void GenerateTeam()
        {
            var imaginarium = DependencyResolver.Current.GetService<Imaginarium>();

            var Repository = DependencyResolver.Current.GetService<IRepository>();

            var coachFirstName = Name.GetRandom();
            var coachLastName = Surname.GetRandom();
            var email = Email.GetRandom(coachFirstName, coachLastName);

            var file = Imaginarium.GetRandomSourceImage();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                //Create user coach
                var user = new User()
                {
                    FirstName = coachFirstName,
                    LastName = coachLastName,
                    AvatarPath = imaginarium.MakePreview(fs, "/Media/files/avatars/", "AvatarSize"),
                    Email = email,
                    Password = "123456",
                    PhoneNumber = Phone.GetRandom(),
                    ActivatedDate = DateTime.Now
                };
                Repository.CreateUser(user);

                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 2 //coach
                };
                Repository.CreateUserRole(userRole);


                var state = Repository.States.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                var school = new School()
                {
                    Name = GenerateData.Team.GetRandom(),
                    StateID = state.ID
                };
                Repository.CreateSchool(school);
                var fileTeam = Imaginarium.GetRandomSourceImage();
                using (var fst = new FileStream(fileTeam, FileMode.Open))
                {
                    //Create team for him
                    var team = new Model.Team()
                    {
                        UserID = user.ID,
                        StateID = state.ID,
                        SchoolID = school.ID,
                        Name = GenerateData.Team.GetRandom(),
                        LogoPath = imaginarium.MakePreview(fs, "/Media/files/logo/", "TeamLogoSize"),
                    };

                    Repository.CreateTeam(team);

                    var groups = new List<Group>();
                    for (int i = 0; i < 4; i++)
                    {
                        var group = new Group()
                        {
                            Name = GenerateData.Team.GetRandom(),
                            TeamID = team.ID,
                        };
                        Repository.CreateGroup(group);
                        groups.Add(group);
                    }

                    var userSeason = new UserSeason()
                    {
                        StartDay = new DateTime(2015, 1, 4),
                        SeasonID = 1,
                        UserID = user.ID,
                    };
                    Repository.CreateUserSeason(userSeason);

                    for (int i = 0; i < 70; i++)
                    {
                        try
                        {
                            CreatePlayer(team.ID, groups.OrderBy(p => Guid.NewGuid()).FirstOrDefault());
                        }
                        catch { }
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        try
                        {
                            CreatePlayer(team.ID, null);
                        }
                        catch { }
                    }
                    Console.WriteLine("Team " + team.Name + " generated");
                }
            }
        }

        private void CreatePlayer(int teamID, Group group)
        {
            var imaginarium = DependencyResolver.Current.GetService<Imaginarium>();

            var Repository = DependencyResolver.Current.GetService<IRepository>();

            var firstName = Name.GetRandom();
            var lastName = Surname.GetRandom();
            var email = Email.GetRandom(firstName, lastName);
            var level = Repository.Levels.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();

            int? gradYear = null;
            if (level.ID == 2 || level.ID == 3)
            {
                gradYear = DateTime.Now.Year + rand.Next(5);
            }
            var file = Imaginarium.GetRandomSourceImage();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                //Create user
                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = "123456",
                    AvatarPath = imaginarium.MakePreview(fs, "/Media/files/avatars/", "AvatarSize"),

                    PhoneNumber = Phone.GetRandom(),
                    ActivatedDate = DateTime.Now,
                    Height = rand.Next(2) + 4 + "'" + rand.Next(12) + "''",
                    Weight = (rand.Next(40) + 80) + " lbs",
                    PlayerOfTeamID = teamID,
                    Birthday = new DateTime(1970, 1, 1).AddDays(rand.Next(35 * 365)),
                    LevelID = level.ID,
                    GradYear = gradYear
                };
                if (group != null)
                {
                    user.GroupID = group.ID;
                }
                Repository.CreateUser(user);

                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 3 //player
                };
                Repository.CreateUserRole(userRole);


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
                var startDate = new DateTime(2015, 1, 4);
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

                startDate = new DateTime(2015, 1, 4);
                while (startDate < DateTime.Now)
                {
                    startDate = startDate.AddDays(rand.Next(3));
                    var userAttendance = new UserAttendance()
                    {
                        UserID = user.ID,
                        AddedDate = startDate,
                        UserSeasonID = user.CurrentSeason.ID
                    };
                    Repository.CreateUserAttendance(userAttendance);
                }
                Console.WriteLine("Player " + user.FirstName + " " + user.LastName + " generated");
            }
        }

        private void GenerateIndividual()
        {
            var imaginarium = DependencyResolver.Current.GetService<Imaginarium>();

            var Repository = DependencyResolver.Current.GetService<IRepository>();

            var firstName = Name.GetRandom();
            var lastName = Surname.GetRandom();
            var email = Email.GetRandom(firstName, lastName);
            var file = Imaginarium.GetRandomSourceImage();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                //Create user coach
                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    AvatarPath = imaginarium.MakePreview(fs, "/Media/files/avatars/", "AvatarSize"),
                    Password = "123456",
                    PhoneNumber = Phone.GetRandom(),
                    ActivatedDate = DateTime.Now,
                    Height = rand.Next(2) + 4 + "'" + rand.Next(12) + "''",
                    Weight = (rand.Next(40) + 80) + " lbs",
                    Birthday = new DateTime(1970, 1, 1).AddDays(rand.Next(35 * 365)),
                };

                Repository.CreateUser(user);

                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 4 //individual
                };
                Repository.CreateUserRole(userRole);

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
                var startDate = new DateTime(2015, 1, 4);

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

                var userSeason = new UserSeason()
                {
                    StartDay = new DateTime(2015, 1, 4),
                    SeasonID = 1,
                    UserID = user.ID,
                };
                Repository.CreateUserSeason(userSeason);
                Console.WriteLine("Individual player " + user.FirstName + " " + user.LastName + " generated");
            }
        }
    }
}
