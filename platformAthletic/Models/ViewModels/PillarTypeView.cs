using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PillarTypeView
    {
        public enum TypeEnum : int
        {
            Reps = 0x01, 
            Time = 0x02
        }

        public int ID { get; set; }

        public int Type {get; set; }

        [Required]
        public string Name {get; set; }

        public string Placeholder { get; set; }

        [Required]
		public string Measure {get; set; }

        [Required]
        public string TextAbove { get; set; }

        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }

        public string Text { get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem() 
                { 
                    Value = ((int)TypeEnum.Reps).ToString(), 
                    Text = TypeEnum.Reps.ToString(), 
                    Selected = Type == (int)TypeEnum.Reps 
                };
                yield return new SelectListItem()
                {
                    Value = ((int)TypeEnum.Time).ToString(),
                    Text = TypeEnum.Time.ToString(),
                    Selected = Type == (int)TypeEnum.Time
                };
            }
        }
    }
}