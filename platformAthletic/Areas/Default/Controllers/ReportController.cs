using platformAthletic.Model;
using platformAthletic.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles = "coach,individual,player")]
    public class ReportController : DefaultController
    {
        [Authorize(Roles = "coach,individual")]
        public ActionResult Index(SearchAttendanceReport search)
        {
            if (search == null)
            {
                search = new SearchAttendanceReport();
            }

            var attendanceReport = new AttendanceReport(search, CurrentUser.OwnTeam);
            return View(attendanceReport);
        }

        public ActionResult FillWeekAttendance()
        {
            var users = Repository.TeamPlayersUsers.ToList();

            var rand = new Random((int)DateTime.Now.Ticks);

            var j = 0;
            foreach (var user in users)
            {
                j++;
                var value = rand.Next(16);
                for (int i = 0; i < 4; i++)
                {
                    if (value % 2 == 1)
                    {
                        Repository.CreateUserAttendance(new UserAttendance()
                        {
                            UserID = user.ID,
                            UserSeasonID = user.CurrentSeason.ID,
                            AddedDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + i)
                        });
                    }
                    value = value >> 1;
                }
            }
            return null;
        }
    }
}
