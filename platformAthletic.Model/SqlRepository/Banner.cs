using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Banner> Banners
        {
            get
            {
                return Db.Banners;
            }
        }

        public bool CreateBanner(Banner instance)
        {
            if (instance.ID == 0)
            {
                Db.Banners.InsertOnSubmit(instance);
                Db.Banners.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateBanner(Banner instance)
        {
            var cache = Db.Banners.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.BannerPlaceID = instance.BannerPlaceID;
				cache.Name = instance.Name;
				cache.Code = instance.Code;
				cache.SourcePath = instance.SourcePath;
				cache.ImagePath = instance.ImagePath;
				cache.InRotation = instance.InRotation;
                cache.Link = instance.Link;
                Db.Banners.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveBanner(int idBanner)
        {
            Banner instance = Db.Banners.FirstOrDefault(p => p.ID == idBanner);
            if (instance != null)
            {
                Db.Banners.DeleteOnSubmit(instance);
                Db.Banners.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}