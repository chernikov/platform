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
    public class LeaderboardController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index(SearchNationalLeaderboard search)
        {
            if (search == null) 
            {
                search = new SearchNationalLeaderboard();
            }
            var nationalLeaderboard = new NationalLeaderboard(search);
            return View(nationalLeaderboard);
        }

        public ActionResult NationalTop(SearchNationalLeaderboard search)
        {
            if (search == null)
            {
                search = new SearchNationalLeaderboard();
            }
            var nationalTopList = new NationalTopList(search);
            return View(nationalTopList);
        }
       
        public ActionResult JsonPlayers(SearchNationalLeaderboard search)
        {
            if (search == null)
            {
                search = new SearchNationalLeaderboard();
            }
            var jsonNationalLeaderboard = new JsonNationalLeaderboard(search);
            return Json(new
            {
                users = jsonNationalLeaderboard.List.Where(p => p.User.PlayerOfTeamID != null).ToList().Select(p => new
                {
                    rank = p.Position,
                    id = p.User.ID,
                    name = p.User.FirstName + " " + p.User.LastName,
                    state = p.User.Team.State.Name,
                    avatar = p.User.FullAvatarPath
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PlayerInfo(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null) 
            {
                return View(user);
            }
            return null;
        }

        public ActionResult School(SearchSchoolLeaderboard search)
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;
            if (team == null)
            {
                return RedirectToAction("Index");
            }
            if (search == null)
            {
                search = new SearchSchoolLeaderboard();
            }
            var leaderBoardTeamInfo = new SchoolLeaderboard( search, team);
            return View(leaderBoardTeamInfo);
        }

        public ActionResult SchoolTop(SearchSchoolLeaderboard search)
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;
            if (team == null)
            {
                return RedirectToAction("Index");
            }
            if (search == null)
            {
                search = new SearchSchoolLeaderboard();
            }
            var nationalTopList = new SchoolTopList(search, team);
            return View(nationalTopList);
        }

        public ActionResult JsonSchoolPlayers(SearchSchoolLeaderboard search)
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;
            if (team == null)
            {
                return RedirectToAction("Index");
            }
            if (search == null)
            {
                search = new SearchSchoolLeaderboard();
            }
            var jsonNationalLeaderboard = new JsonSchoolLeaderboard(search, team);
            return Json(new
            {
                users = jsonNationalLeaderboard.List.Where(p => p.User.PlayerOfTeamID != null).ToList().Select(p => new
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
