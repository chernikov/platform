using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class TrainingEquipmentView
    {

        public int ID { get; set; }

        public int TrainingSetID { get; set; }

        public int TrainingID { get; set; }

        public int? EquipmentID { get; set; }

        public int? Equipment2ID { get; set; }

        public int Priority { get; set; }

        private IEnumerable<Training> Trainings
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Trainings.OrderBy(p => p.Name).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListTrainingID
        {
            get
            {
                return Trainings.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == TrainingID
                                                    });
            }
        }

        private IEnumerable<Equipment> Equipments
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Equipments.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListEquipmentID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "None",
                    Selected = !EquipmentID.HasValue
                };

                foreach (var equipment in Equipments)
                {
                    yield return new SelectListItem
                    {
                        Value = equipment.ID.ToString(),
                        Text = equipment.Name,
                        Selected = equipment.ID == EquipmentID
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> SelectListEquipment2ID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "None",
                    Selected = !Equipment2ID.HasValue
                };

                foreach (var equipment in Equipments)
                {
                    yield return new SelectListItem
                    {
                        Value = equipment.ID.ToString(),
                        Text = equipment.Name,
                        Selected = equipment.ID == Equipment2ID
                    };
                }
            }
        }
    }
}