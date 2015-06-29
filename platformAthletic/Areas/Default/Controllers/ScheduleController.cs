using platformAthletic.Attributes;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach,individual,assistant")]
    public class ScheduleController : DefaultController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Group()
        {
            var team = CurrentUser.OwnTeam;

            return View(team);
        }

        public ActionResult Scheduling()
        {
            return View(CurrentUser.OwnTeam);
        }

        public ActionResult PersonalScheduling()
        {
            return View(CurrentUser.OwnTeam);
        }

        public ActionResult CreateGroup()
        {
            var groupView = new GroupView();

            return View("EditGroup", groupView);
        }

        [HttpGet]
        public ActionResult EditGroup(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            var groupView = (GroupView)ModelMapper.Map(group, typeof(Group), typeof(GroupView));
            return View(groupView);
        }

        [HttpPost]
        public ActionResult EditGroup(GroupView groupView)
        {
            var exist = Repository.Groups.Any(p => p.TeamID == CurrentUser.OwnTeam.ID && string.Compare(groupView.Name, p.Name, true) == 0 && p.ID != groupView.ID);
            if (exist)
            {
                ModelState.AddModelError("Name", "This group already exists. Please enter another name.");
            }
            if (ModelState.IsValid)
            {
                var group = (Group)ModelMapper.Map(groupView, typeof(GroupView), typeof(Group));

                if (group.ID == 0)
                {
                    group.TeamID = CurrentUser.OwnTeam.ID;
                    Repository.CreateGroup(group);
                }
                else
                {
                    Repository.UpdateGroup(group);
                }
                return View("_OK");
            }
            return View(groupView);
        }

        [HttpGet]
        public ActionResult RemoveGroup(int id)
        {
            var group = Repository.Groups.FirstOrDefault(p => p.ID == id);
            var groupView = (GroupView)ModelMapper.Map(group, typeof(Group), typeof(GroupView));
            return View(groupView);
        }

        [HttpPost]
        public ActionResult RemoveGroup(GroupView groupView)
        {
            Repository.RemoveGroup(groupView.ID);
            return View("_OK");
        }

        public ActionResult AssignPlayers(int[] idUsers, int groupId)
        {
            if (idUsers != null)
            {
                foreach (var id in idUsers)
                {
                    var user = Repository.Users.FirstOrDefault(p => p.ID == id);
                    user.GroupID = groupId != 0 ? (int?)groupId : null;

                    Repository.ChangeGroup(user);
                }
            }
            return View("_OK");
        }

        public ActionResult Calendar(DateTime month, int teamId, int? groupId = null)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);
            if (team != null)
            {
                var currentSeason = team.User.SeasonByDateAndGroup(month, groupId, true);
                var seasons = Repository.Seasons.ToList();
                int numberOfWeek = (int)(((int)(month - currentSeason.StartDay).TotalDays) / 7);
                int totalWeeks = currentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
                numberOfWeek = (totalWeeks + numberOfWeek) % totalWeeks + 1;

                var calendarInfo = new CalendarInfo()
                {
                    Season = currentSeason.Season,
                    Seasons = seasons,
                    Month = month,
                    Team = team
                };

                if (groupId != null)
                {
                    var group = Repository.Groups.FirstOrDefault(p => p.ID == groupId);
                    calendarInfo.Group = group;
                }

                return View(calendarInfo);
            }
            return null;
        }

        public ActionResult CalendarRow(DateTime date, int teamId, int? groupId = null)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);
            if (team != null)
            {
                var currentSeason = team.User.SeasonByDateAndGroup(date, groupId, true);
                int numberOfWeek = (int)(((int)(date - currentSeason.StartDay).TotalDays) / 7);
                int totalWeeks = currentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
                numberOfWeek = numberOfWeek % totalWeeks + 1;
                if ((date - currentSeason.StartDay).TotalDays < 0)
                {
                    numberOfWeek--;
                }
                var week = Repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == currentSeason.SeasonID);
                if (week == null)
                {
                    return View(new CalendarRowInfo()
                    {
                        CurrentSunday = date
                    });
                }
                var macrocycle = week.Macrocycles.FirstOrDefault();
                var calendarRowInfo = new CalendarRowInfo()
                {
                    CurrentSunday = date,
                    Macrocycle = macrocycle,
                    NumberOfWeek = numberOfWeek,
                    IsDefault = true
                };
                if (groupId.HasValue)
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == teamId && p.GroupID == groupId && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        calendarRowInfo.Macrocycle = schedule.Macrocycle;
                        calendarRowInfo.IsDefault = false;
                    }
                }
                else
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == teamId && p.GroupID == null && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        calendarRowInfo.Macrocycle = schedule.Macrocycle;
                        calendarRowInfo.IsDefault = false;
                    }
                }
                return View(calendarRowInfo);
            }
            return null;
        }

        public ActionResult SetSchedule(int number, int macrocycleId, int teamId, int? groupId, DateTime date)
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }

            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);

            var currentSeason = team.User.SeasonByDateAndGroup(date, groupId, true);
            var schedule = new Schedule()
            {
                Number = number,
                UserSeasonID = currentSeason.ID,
                MacrocycleID = macrocycleId,
                TeamID = teamId,
                GroupID = groupId
            };
            Repository.CreateSchedule(schedule);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartCycle(int number, int cycleId, int teamId, int? groupId, DateTime date)
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }

            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);

            var currentSeason = team.User.SeasonByDateAndGroup(date, groupId, true);
            var cycle = Repository.Cycles.FirstOrDefault(p => p.ID == cycleId);

            foreach (var macrocycle in cycle.Macrocycles)
            {
                var schedule = new Schedule()
                {
                    Number = number,
                    UserSeasonID = currentSeason.ID,
                    MacrocycleID = macrocycle.ID,
                    TeamID = teamId,
                    GroupID = groupId
                };
                number++;
                Repository.CreateSchedule(schedule);
            }

            

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartSeason(int seasonId, DateTime date, int teamId, int? groupId)
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }

            var team = Repository.Teams.FirstOrDefault(p => p.ID == teamId);
            var futureSeasons = Repository.UserSeasons.Where(p => p.UserID == team.UserID && p.StartDay >= date);
            if (groupId != null)
            {
                futureSeasons = futureSeasons.Where(p => p.GroupID == groupId);
            }
            foreach (var season in futureSeasons.ToList())
            {
                foreach (var schedule in season.Schedules.ToList())
                {
                    Repository.RemoveSchedule(schedule.ID);
                }
                Repository.RemoveUserSeason(season.ID);
            }
            var newUserSeason = new UserSeason()
            {
                SeasonID = seasonId,
                UserID = team.UserID,
                GroupID = groupId,
                StartDay = date,
            };
            Repository.CreateUserSeason(newUserSeason);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetPersonalSchedule(int number, int macrocycleId, DateTime date)
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }

            var currentSeason = CurrentUser.SeasonByDateAndGroup(date);
            var personalSchedule = new PersonalSchedule()
            {
                Number = number,
                MacrocycleID = macrocycleId,
                UserID = CurrentUser.ID,
                UserSeasonID = currentSeason.ID
            };

            Repository.CreatePersonalSchedule(personalSchedule);

            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartPersonalCycle(int number, int cycleId,  DateTime date)
        {

            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }

            var currentSeason = CurrentUser.SeasonByDateAndGroup(date);
            var cycle = Repository.Cycles.FirstOrDefault(p => p.ID == cycleId);

            foreach (var macrocycle in cycle.Macrocycles)
            {
                var personalSchedule = new PersonalSchedule()
                {
                    Number = number,
                    UserSeasonID = currentSeason.ID,
                    MacrocycleID = macrocycle.ID,
                };
                number++;
                Repository.CreatePersonalSchedule(personalSchedule);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartPersonalSeason(int seasonId, DateTime date)
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ConfirmTrainingStartDate);
            }
            
            var futureSeasons = Repository.UserSeasons.Where(p => p.UserID == CurrentUser.ID && p.StartDay > date);
            foreach (var season in futureSeasons.ToList())
            {
                foreach (var schedule in season.Schedules.ToList())
                {
                    Repository.RemoveSchedule(schedule.ID);
                }
                Repository.RemoveUserSeason(season.ID);
            }
            var newUserSeason = new UserSeason()
            {
                SeasonID = seasonId,
                UserID = CurrentUser.ID,
                StartDay = date,
            };
            Repository.CreateUserSeason(newUserSeason);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetSchedule(int id = 0, int? groupId = null)
        {
            ViewBag.GroupId = groupId;
            return View();
        }

        public ActionResult PersonalCalendar(DateTime month)
        {
            var currentSeason = CurrentUser.SeasonByDateAndGroup(month);
            var seasons = Repository.Seasons.ToList();
            int numberOfWeek = (int)(((int)(month - currentSeason.StartDay).TotalDays) / 7);
            int totalWeeks = currentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
            numberOfWeek = numberOfWeek % totalWeeks + 1;
            var calendarInfo = new PersonalCalendarInfo()
            {
                Season = currentSeason.Season,
                Month = month,
                User = CurrentUser,
                Seasons = seasons
            };

            return View(calendarInfo);
        }

        public ActionResult PersonalCalendarRow(DateTime date)
        {
            var currentSeason = CurrentUser.SeasonByDateAndGroup(date);

            int numberOfWeek = (int)(((int)(date - currentSeason.StartDay).TotalDays) / 7);
            int totalWeeks = currentSeason.Season.Cycles.SelectMany(p => p.Phases).SelectMany(p => p.Weeks).Where(p => p.Number != null).Count();
            numberOfWeek = numberOfWeek % totalWeeks + 1;
            if ((date - currentSeason.StartDay).TotalDays < 0)
            {
                numberOfWeek--;
            }
            var week = Repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == currentSeason.SeasonID);
            if (week == null)
            {
                return View(new CalendarRowInfo()
                {
                    CurrentSunday = date
                });
            }
            var macrocycle = week.Macrocycles.FirstOrDefault();
            var calendarRowInfo = new CalendarRowInfo()
            {
                CurrentSunday = date,
                Macrocycle = macrocycle,
                NumberOfWeek = numberOfWeek
            };
            var schedule = Repository.PersonalSchedules.FirstOrDefault(p => p.Number == numberOfWeek && p.UserID == CurrentUser.ID && p.UserSeasonID == currentSeason.ID);
            if (schedule != null)
            {
                calendarRowInfo.Macrocycle = schedule.Macrocycle;
            }
            return View(calendarRowInfo);
        }

    }
}
