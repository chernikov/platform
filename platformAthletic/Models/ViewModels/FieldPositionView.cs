using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class FieldPositionView
    {
        public int ID { get; set; }

        [Required]
        public string Code { get; set; }

        public int SportID { get; set; }

        private IEnumerable<Sport> Sports
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Sports.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListSportID
        {
            get
            {
                return Sports.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name,
                    Selected = p.ID == SportID
                });
            }
        }


        [Required]
        public string Name { get; set; }

    }
}