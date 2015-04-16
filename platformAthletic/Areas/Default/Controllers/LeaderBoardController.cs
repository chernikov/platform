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
    public class LeaderBoardController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index(SearchNationalLeaderBoard searchNationalLeaderBoard)
        {
            if (searchNationalLeaderBoard == null) 
            {
                searchNationalLeaderBoard = new SearchNationalLeaderBoard();
            }
            ViewBag.Search = searchNationalLeaderBoard;

            var leaderBoardNationalInfo = new LeaderBoardNationalInfo(searchNationalLeaderBoard);
            return View(leaderBoardNationalInfo);
        }

        public ActionResult Team()
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;

            var leaderBoardTeamInfo = new LeaderBoardTeamInfo(team);
            return View(leaderBoardTeamInfo);
        }

        public ActionResult JsonPlayers(SearchNationalLeaderBoard searchNationalLeaderBoard)
        {
            if (searchNationalLeaderBoard == null)
            {
                searchNationalLeaderBoard = new SearchNationalLeaderBoard();
            }
            var leaderBoardNationalInfo = new LeaderBoardNationalInfo(searchNationalLeaderBoard, true);
            return Json(new
            {
                users = leaderBoardNationalInfo.List.Where(p => p.User.PlayerOfTeamID != null).ToList().Select(p => new
                {
                    rank = p.Position,
                    id = p.User.ID,
                    name = p.User.FirstName + " " + p.User.LastName,
                    state = p.User.Team.State.Name,
                    avatar = p.User.FullAvatarPath
                })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
