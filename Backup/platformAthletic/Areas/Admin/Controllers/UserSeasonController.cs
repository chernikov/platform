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
                    var now = DateTime.Now;
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
                int numberOfWeek = (int)((DateTime.Now - StartDay).TotalDays / 7) + 1;
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
    }
}