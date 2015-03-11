using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<BannerPlace> BannerPlaces
        {
            get
            {
                return Db.BannerPlaces;
            }
        }

        public bool CreateBannerPlace(BannerPlace instance)
        {
            if (instance.ID == 0)
            {
                Db.BannerPlaces.InsertOnSubmit(instance);
                Db.BannerPlaces.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBannerPlace(BannerPlace instance)
        {
            var cache = Db.BannerPlaces.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Height = instance.Height;
				cache.Width = instance.Width;
                Db.BannerPlaces.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBannerPlace(int idBannerPlace)
        {
            BannerPlace instance = Db.BannerPlaces.FirstOrDefault(p => p.ID == idBannerPlace);
            if (instance != null)
            {
                Db.BannerPlaces.DeleteOnSubmit(instance);
                Db.BannerPlaces.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}