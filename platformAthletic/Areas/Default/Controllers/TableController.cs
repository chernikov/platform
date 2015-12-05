using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Helpers;
using platformAthletic.Models.Info;
using platformAthletic.Model;
using platformAthletic.Attributes;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize]
    public class TableController : DefaultController
    {

        [Authorize(Roles = "coach,assistant")]
        public ActionResult Team(int? groupId, int[] idUsers)
        {
            IQueryable<User> list;
            if (idUsers == null)
            {
                if (!groupId.HasValue)
                {
                    list = Repository.Users.Where(p => p.PlayerOfTeamID == CurrentUser.OwnTeam.ID);
                }
                else
                {
                    list = Repository.Users.Where(p => p.GroupID == groupId);
                }

                if (list.Any() == false)
                {
                    return null;
                }

                if (list.Any() == true && CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
                {
                    try
                    {
                        Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ViewWorkOut);
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                if (idUsers == null)
                {
                    return null;
                }
                if (idUsers.Count() > 0 && CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
                {
                    try
                    {
                        Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ViewWorkOut);
                    }
                    catch (Exception ex) { }
                }

                list = Repository.Users.Where(p => idUsers.Contains(p.ID));
            }

            var listTables = new List<TableInfo>();
            foreach (var user in list)
            {
                var currentSeason = user.CurrentSeason;
                if (DateTime.Now.Current() < currentSeason.StartDay)
                {
                    continue;
                }
                int numberOfWeek = currentSeason.NumberOfWeek(DateTime.Now.Current());
                var week = Repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == currentSeason.SeasonID);
                var macrocycle = week.Macrocycle;
                if (user.GroupID.HasValue)
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == user.PlayerOfTeamID && p.GroupID == user.GroupID && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        macrocycle = schedule.Macrocycle;
                    }
                }
                else
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == user.PlayerOfTeamID && p.GroupID == null && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        macrocycle = schedule.Macrocycle;
                    }
                }
                if (macrocycle != null)
                {
                    var tableInfo = new TableInfo()
                    {
                        Days = Repository.TrainingDays.Where(p => p.MacrocycleID == macrocycle.ID).OrderBy(p => p.ID).ToList(),
                        Equipments = CurrentUser.Equpments.Select(p => p.ID).ToList(),
                        User = user
                    };
                    listTables.Add(tableInfo);
                }
            }
            return View(listTables);
        }

        public ActionResult Index()
        {
            if (CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.ViewWorkOut);
            }
            var currentSeason = CurrentUser.CurrentSeason;

            if (DateTime.Now.Current() < currentSeason.StartDay)
            {
                ViewBag.StartDate = currentSeason.StartDay;
                return View("NoData");
            }
            int numberOfWeek = currentSeason.NumberOfWeek(DateTime.Now.Current());
            var week = Repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == currentSeason.SeasonID);
            var macrocycle = week.Macrocycle;
            if (CurrentUser.InRoles("individual"))
            {
                var schedule = Repository.PersonalSchedules.FirstOrDefault(p => p.Number == numberOfWeek && p.UserID == CurrentUser.ID && p.UserSeasonID == currentSeason.ID);
                if (schedule != null)
                {
                    macrocycle = schedule.Macrocycle;
                }
            }
            else
            {
                if (CurrentUser.GroupID.HasValue)
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == CurrentUser.PlayerOfTeamID && p.GroupID == CurrentUser.GroupID && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        macrocycle = schedule.Macrocycle;
                    }
                }
                else
                {
                    var schedule = Repository.Schedules.FirstOrDefault(p => p.Number == numberOfWeek && p.TeamID == CurrentUser.PlayerOfTeamID && p.GroupID == null && p.UserSeasonID == currentSeason.ID);
                    if (schedule != null)
                    {
                        macrocycle = schedule.Macrocycle;
                    }
                }
            }
            if (macrocycle != null)
            {
                var tableInfo = new TableInfo()
                {
                    Days = Repository.TrainingDays.Where(p => p.MacrocycleID == macrocycle.ID).OrderBy(p => p.ID).ToList(),
                    Equipments = CurrentUser.Equpments.Select(p => p.ID).ToList(),
                    User = CurrentUser
                };
                return View(tableInfo);
            }


            return Content("Can't generate WWS");
        }

        public ActionResult Preview(int macrocycleId)
        {
            var macrocycle = Repository.Macrocycles.FirstOrDefault(p => p.ID == macrocycleId);
            if (macrocycle != null)
            {
                if (CurrentUser.InRoles("Individual"))
                {
                    var tableInfo = new TableInfo()
                    {
                        Days = Repository.TrainingDays.Where(p => p.MacrocycleID == macrocycle.ID).OrderBy(p => p.ID).ToList(),
                        Equipments = CurrentUser.Equpments.Select(p => p.ID).ToList(),
                        User = CurrentUser
                    };
                    return View(tableInfo);
                }
                else
                {
                    var tableInfo = new TableInfo()
                    {
                        Days = Repository.TrainingDays.Where(p => p.MacrocycleID == macrocycle.ID).OrderBy(p => p.ID).ToList(),
                        Equipments = CurrentUser.Equpments.Select(p => p.ID).ToList(),
                        User = new User()
                        {
                            Squat = 300,
                            Bench = 250,
                            Clean = 225,
                            FirstName = "John",
                            LastName = "Smith"
                        }
                    };
                    return View(tableInfo);
                }
            }
            return Content("Can't generate WWS");
        }

        private DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}
