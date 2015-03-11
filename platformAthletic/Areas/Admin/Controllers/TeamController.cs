using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class TeamController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.Teams.OrderBy(p => p.Name).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var team = Repository.Teams.FirstOrDefault(p => p.ID == id);

            if (team != null)
            {
                var teamView = (TeamView)ModelMapper.Map(team, typeof(Team), typeof(TeamView));
                return View(teamView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(TeamView teamView)
        {
            if (ModelState.IsValid)
            {
                var team = (Team)ModelMapper.Map(teamView, typeof(TeamView), typeof(Team));

                Repository.UpdateTeamCount(team);
                return RedirectToAction("Index");
            }
            return View(teamView);
        }
    }
}