using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{
    public class StateController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.States.OrderBy(p => p.Name).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var stateView = new StateView();
            return View("Edit", stateView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var state = Repository.States.FirstOrDefault(p => p.ID == id);

            if (state != null)
            {
                var stateView = (StateView)ModelMapper.Map(state, typeof(State), typeof(StateView));
                return View(stateView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(StateView stateView)
        {
            if (ModelState.IsValid)
            {
                var state = (State)ModelMapper.Map(stateView, typeof(StateView), typeof(State));
                if (state.ID == 0)
                {
                    Repository.CreateState(state);
                }
                else
                {
                    Repository.UpdateState(state);
                }
                return RedirectToAction("Index");
            }
            return View(stateView);
        }

        public ActionResult Delete(int id)
        {
            var state = Repository.States.FirstOrDefault(p => p.ID == id);
            if (state != null)
            {
                Repository.RemoveState(state.ID);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditBatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditBatch(string list)
        {
            var states = list.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in states)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var split = item.Split(new string[] { "\t" }, StringSplitOptions.None);

                    var state = new State()
                    {
                        Name = split[0],
                        Code = split[1]
                    };
                    Repository.CreateState(state);
                }

            }
            return null;
        }
    }
}