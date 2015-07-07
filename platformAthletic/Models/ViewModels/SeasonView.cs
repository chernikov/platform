using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class SeasonView
    {
        public int ID { get; set; }

		public int Type {get; set; }

        public IEnumerable<SelectListItem> SelectListType
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = ((int)Season.TypeEnum.OffSeason).ToString(),
                    Text = (Season.TypeEnum.OffSeason).ToString(),
                    Selected = Type == (int)Season.TypeEnum.OffSeason
                };

                yield return new SelectListItem()
                {
                    Value = ((int)Season.TypeEnum.InSeason).ToString(),
                    Text = (Season.TypeEnum.InSeason).ToString(),
                    Selected = Type == (int)Season.TypeEnum.InSeason
                };
            }
        }

		public string Name {get; set; }
    }
}