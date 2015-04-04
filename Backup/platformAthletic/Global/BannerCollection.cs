using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using platformAthletic.Model;

namespace platformAthletic.Global
{
    public class BannerCollection
    {
        [Inject]
        public IRepository Repository { get; set; }

        public Banner this[string index]
        {
            get
            {
                var banners = Repository.Banners.Where(p => p.InRotation && string.Compare(p.BannerPlace.Name, index, true) == 0);
                return banners.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            }
        }
    }
}