using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class VideoView
    {
        public int ID { get; set; }

        public int? TrainingID { get; set; }

        [Required(ErrorMessage="Enter Header")]
        public string Header { get; set; }

        public string Text { get; set; }

        [Required(ErrorMessage = "Enter youtube link")]
        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }

        private IEnumerable<Training> Trainings
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Trainings.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListTrainingID
        {
            get
        	{
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "None",
                    Selected = !TrainingID.HasValue
                };

                foreach(var training in Trainings) 
                {
                    yield return new SelectListItem
                                    {
                                        Value = training.ID.ToString(),
                                        Text = training.Name,
                                        Selected = training.ID == TrainingID
                                    };
                }
        	}
        }

    }
}