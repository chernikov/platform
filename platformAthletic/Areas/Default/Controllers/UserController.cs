using platformAthletic.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class UserController : DefaultController
    {
        public ActionResult Index(int id)
        {
            var user = Repository.TeamPlayersUsers.FirstOrDefault(p => p.ID == id);
            var rankInfo = new RankInfo(user);
            ViewBag.RankInfo = rankInfo;
            return View(user);
        }

        public ActionResult Last12WeekPerformance(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                var sunday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                var currentSunday = sunday.AddDays(-7 * 12);
                var labels = new List<string>();
                var sData = new List<int>();
                var bData = new List<int>();
                var cData = new List<int>();
                var tData = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    var sbc = user.SBCHistory(currentSunday);
                    if (sbc != null)
                    {
                        labels.Add(currentSunday.ToString("MMM/dd"));
                        sData.Add((int)sbc.Squat);
                        bData.Add((int)sbc.Bench);
                        cData.Add((int)sbc.Clean);
                        tData.Add((int)(sbc.Squat + sbc.Bench + sbc.Clean));
                        currentSunday = currentSunday.AddDays(7);
                    }
                };
                var datasets = new List<PerformanceGraphInfo>();
                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Squat",
                    fillColor = "transparent",
                    strokeColor = "#ed4848",
                    pointColor = "#ed4848",
                    pointStrokeColor = "#ed4848",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = sData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Bench",
                    fillColor = "transparent",
                    strokeColor = "#3bcb67",
                    pointColor = "#3bcb67",
                    pointStrokeColor = "#3bcb67",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = bData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Clean",
                    fillColor = "transparent",
                    strokeColor = "#60b1c2",
                    pointColor = "#60b1c2",
                    pointStrokeColor = "#60b1c2",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = cData
                });

                datasets.Add(new PerformanceGraphInfo()
                {
                    label = "Total",
                    fillColor = "transparent",
                    strokeColor = "#495b6c",
                    pointColor = "#495b6c",
                    pointStrokeColor = "#495b6c",
                    pointHighlightFill = "#fff",
                    pointHighlightStroke = "#fff",
                    datasetFill = false,
                    data = tData
                });
                var data = new
                {
                    labels,
                    datasets
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}