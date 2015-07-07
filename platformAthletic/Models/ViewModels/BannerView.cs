using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{
    public class BannerView
    {
        public int ID { get; set; }

       
        public int BannerPlaceID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string SourcePath { get; set; }

        public string ImagePath { get; set; }

        public string Link { get; set; }

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