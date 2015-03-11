using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class BannerView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [DropDownField]
        public int BannerPlaceID { get; set; }

        [ShowIndex]
        [TextBoxField]
        public string Name { get; set; }

        [TextAreaField]
        public string Code { get; set; }

        [HiddenField]
        public string SourcePath { get; set; }

        [HiddenField]
        public string ImagePath { get; set; }

        public string Link { get; set; }

        [CheckBoxField]
        public bool InRotation { get; set; }

        private IEnumerable<BannerPlace> BannerPlaces
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.BannerPlaces.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListBannerPlaceID
        {
            get
            {
                return BannerPlaces.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == BannerPlaceID
                                                    });
            }
        }

    }
}