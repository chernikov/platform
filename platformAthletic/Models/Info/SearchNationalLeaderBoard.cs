using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class SearchNationalLeaderBoard
    {
        public int StateID { get; set; }

        public int TrainingType { get; set; }


        private IEnumerable<State> States
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.States.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListStateID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "All",
                    Selected = StateID == 0
                };

                foreach (var state in States)
                {
                    yield return new SelectListItem
                    {
                        Value = state.ID.ToString(),
                        Text = state.Code,
                        Selected = state.ID == StateID
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> SelectListTrainingType
        {
            get
            {
                yield return new SelectListItem { Value = "", Text = "All", Selected = TrainingType == 0 };
                yield return new SelectListItem { Value = "1", Text = "Squat", Selected = TrainingType == 1 };
                yield return new SelectListItem { Value = "2", Text = "Bench", Selected = TrainingType == 2 };
                yield return new SelectListItem { Value = "3", Text = "Clean", Selected = TrainingType == 3 };
            }
        }
    }
}