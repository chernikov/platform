using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.Info;
using platformAthletic.Attributes;

namespace platformAthletic.Areas.Default.Controllers
{
    [Authorize(Roles="coach,player")]
    public class OldLeaderBoardController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index(SearchNationalLeaderboard searchNationalLeaderBoard)
        {
            if (searchNationalLeaderBoard == null) 
            {
                searchNationalLeaderBoard = new SearchNationalLeaderboard();
            }
            ViewBag.Search = searchNationalLeaderBoard;

            var leaderBoardNationalInfo = new NationalLeaderboard(searchNationalLeaderBoard);
            return View(leaderBoardNationalInfo);
        }
        /*
        public ActionResult Team()
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;

            var leaderBoardTeamInfo = new LeaderBoardTeamInfo(team);
            return View(leaderBoardTeamInfo);
        }*/
    }
}
