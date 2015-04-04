using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class SeasonView
    {
        [PrimaryField]
        public int ID { get; set; }

        [DropDownField]
        [ShowIndex]
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

        [TextBoxField]
        [ShowIndex]
		public string Name {get; set; }
    }
}