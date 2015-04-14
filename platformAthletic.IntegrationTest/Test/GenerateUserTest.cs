using GenerateData;
using NUnit.Framework;
using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace platformAthletic.IntegrationTest.Test
{

    [TestFixture]
    public class GenerateUserTest
    {
        [Test]
        public void GenerateMoreThanOne()
        {
            for (int i = 0; i < 2; i++)
            {
                Generate();
            }
        }
        
        private void Generate()
        {
            var Repository = DependencyResolver.Current.GetService<IRepository>();

            var coachFirstName = Name.GetRandom();
            var coachLastName = Surname.GetRandom();
            var email = Email.GetRandom(coachFirstName, coachLastName);

            //Create user coach
            var user = new User()
            {
                FirstName = coachFirstName,
                LastName = coachLastName,
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
            //Create team for him
            var team = new Model.Team() {
                UserID = user.ID, 
                StateID = state.ID,
                SchoolID = school.ID,
                Name = GenerateData.Team.GetRandom(),
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
                CreatePlayer(team.ID, groups.OrderBy(p => Guid.NewGuid()).FirstOrDefault());
            }
            for (int i = 0; i < 30; i++)
            {
                CreatePlayer(team.ID, null);
            }

        }

        private void CreatePlayer(int teamID, Group group)
        {
               var Repository = DependencyResolver.Current.GetService<IRepository>();

            var firstName = Name.GetRandom();
            var lastName = Surname.GetRandom();
            var email = Email.GetRandom(firstName, lastName);

            //Create user
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = "123456",
                PhoneNumber = Phone.GetRandom(),
                ActivatedDate = DateTime.Now,
                Height = "5'8''",
                Weight = "160 lbs",
                PlayerOfTeamID = teamID, 
                Birthday = new DateTime(1990, 1, 1),
            };

            if (group != null)
            {
                user.GroupID = group.ID;
            }
            Repository.CreateUser(user);

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

            var rand = new Random();
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
        }
    }
}
