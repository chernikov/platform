using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;
using platformAthletic.Helpers;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class UserSeasonController : AdminController
    {
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userseason = Repository.UserSeasons.FirstOrDefault(p => p.ID == id);

            if (userseason != null)
            {
                var userseasonView = (UserSeasonView)ModelMapper.Map(userseason, typeof(UserSeason), typeof(UserSeasonView));
                userseasonView.Init();
                return View(userseasonView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(UserSeasonView userseasonView)
        {
            if (ModelState.IsValid)
            {
                var userseason = (UserSeason)ModelMapper.Map(userseasonView, typeof(UserSeasonView), typeof(UserSeason));
                Repository.UpdateUserSeason(userseason);
                return RedirectToAction("Index", "User");
            }
            return View(userseasonView);
        }

        public ActionResult Delete(int id)
        {
            var userseason = Repository.UserSeasons.FirstOrDefault(p => p.ID == id);
            if (userseason != null)
            {
                Repository.RemoveUserSeason(userseason.ID);
            }
            return RedirectToAction("Index", "User");
        }

        public ActionResult UpdateByWeek(int? WeekID)
        {
            if (WeekID.HasValue)
            {
                var week = Repository.Weeks.FirstOrDefault(p => p.ID == WeekID);
                if (week != null)
                {
                    var seasonID = week.Phase.Cycle.Season.ID;
                    var now = DateTime.Now.Current();
                    var startDay = now.AddDays(-(int)now.DayOfWeek).AddDays(-7 * (week.Number.Value - 1));
                    return Json(new
                    {
                        result = "ok",
                        startDay = startDay.ToString("d"),
                        seasonID
                    });
                }
            }
            return Json(new { result = "error" });
        }

        public ActionResult UpdateBySeasonAndDate(DateTime StartDay, int SeasonID)
        {
            var season = Repository.Seasons.FirstOrDefault(p => p.ID == SeasonID);
            if (season != null)
            {
                int numberOfWeek = (int)((DateTime.Now.Current() - StartDay).TotalDays / 7) + 1;
                var week = Repository.Weeks.FirstOrDefault(p => p.Number == numberOfWeek && p.Phase.Cycle.SeasonID == SeasonID);
                if (week != null)
                {
                    return Json(new
                    {
                        result = "ok",
                        WeekID = week.ID,
                    });
                }
            }
            return Json(new
            {
                result = "ok",
                WeekID = ""
            });
        }

        public ActionResult FixScheduleToUserSeason()
        {
            FixSchedule();
            FixPersonalSchedule();
            return null;
        }

        private void FixSchedule()
        {
            var schedules = Repository.Schedules.ToList();
            var enumerator = schedules.GetEnumerator();
            var set = new List<Schedule>();
            int? checksum = null;
            var number = 0;
            enumerator.MoveNext();
            var current = enumerator.Current;
            while (current != null)
            {
                if (checksum == current.Number - current.ID && current.Macrocycle.Week.Number != null)
                {
                    set.Add(current);
                }
                else
                {
                    if (checksum != null)
                    {
                        if (set.Count > 2)
                        {
                            //save previous
                            var setFirst = set.First();
                            var userSeason = new UserSeason()
                            {
                                SeasonID = setFirst.UserSeason.SeasonID,
                                UserID = setFirst.Team.UserID,
                                StartDay = setFirst.UserSeason.StartDay.AddDays(number * 7),
                                StartFrom = setFirst.Macrocycle.Week.Number.Value,
                                GroupID = setFirst.GroupID,

                            };
                            Repository.CreateUserSeason(userSeason);
                            checksum = null;
                            number = 0;
                            foreach (var schedule in set)
                            {
                                Repository.RemoveSchedule(schedule.ID);
                            }
                            set.Clear();
                        }
                        else
                        {
                            foreach (var schedule in set)
                            {
                                var numberWeek = schedule.Number;
                                var seasonStartDay = schedule.UserSeason.StartDay;
                                schedule.Date = seasonStartDay.AddDays(7 * numberWeek);
                                Repository.UpdateSchedule(schedule);
                            }
                            set.Clear();
                        }
                    }
                    checksum = current.Number - current.ID;
                    if (current.Macrocycle.Week.Number != null)
                    {
                        set.Add(current);

                    }
                    else
                    {
                        var numberWeek = current.Number;
                        var seasonStartDay = current.UserSeason.StartDay;
                        current.Date = seasonStartDay.AddDays(7 * numberWeek);
                        Repository.UpdateSchedule(current);
                    }
                }
                enumerator.MoveNext();
                current = enumerator.Current;
            }

        }

        private void FixPersonalSchedule()
        {
            var schedules = Repository.PersonalSchedules.ToList();
            var enumerator = schedules.GetEnumerator();
            var set = new List<PersonalSchedule>();
            int? checksum = null;
            var number = 0;
            enumerator.MoveNext();
            var current = enumerator.Current;
            while (current != null)
            {
                if (checksum == current.Number - current.ID && current.Macrocycle.Week.Number != null)
                {
                    set.Add(current);
                }
                else
                {
                    if (checksum != null)
                    {
                        if (set.Count > 2)
                        {
                            //save previous
                            var setFirst = set.First();
                            var userSeason = new UserSeason()
                            {
                                SeasonID = setFirst.UserSeason.SeasonID,
                                UserID = setFirst.UserID,
                                StartDay = setFirst.UserSeason.StartDay.AddDays(number * 7),
                                StartFrom = setFirst.Macrocycle.Week.Number.Value,
                            };
                            Repository.CreateUserSeason(userSeason);
                            checksum = null;
                            number = 0;
                            foreach (var schedule in set)
                            {
                                Repository.RemovePersonalSchedule(schedule.ID);
                            }
                            set.Clear();
                        }
                        else
                        {
                            foreach (var schedule in set)
                            {
                                var numberWeek = schedule.Number;
                                var seasonStartDay = schedule.UserSeason.StartDay;
                                schedule.Date = seasonStartDay.AddDays(7 * numberWeek);
                                Repository.UpdatePersonalSchedule(schedule);
                            }
                            set.Clear();
                        }
                    }
                    checksum = current.Number - current.ID;
                    if (current.Macrocycle.Week.Number != null)
                    {
                        set.Add(current);

                    }
                    else
                    {
                        var numberWeek = current.Number;
                        var seasonStartDay = current.UserSeason.StartDay;
                        current.Date = seasonStartDay.AddDays(7 * numberWeek);
                        Repository.UpdatePersonalSchedule(current);
                    }
                }
                enumerator.MoveNext();
                current = enumerator.Current;
            }
        }
    }
}