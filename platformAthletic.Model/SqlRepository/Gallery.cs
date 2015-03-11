using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Gallery> Galleries
        {
            get
            {
                return Db.Galleries;
            }
        }

        public bool CreateGallery(Gallery instance)
        {
            if (instance.ID == 0)
            {
                Db.Galleries.InsertOnSubmit(instance);
                Db.Galleries.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGallery(Gallery instance)
        {
            var cache = Db.Galleries.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.ImagePath = instance.ImagePath;
                Db.Galleries.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGallery(int idGallery)
        {
            Gallery instance = Db.Galleries.FirstOrDefault(p => p.ID == idGallery);
            if (instance != null)
            {
                Db.Galleries.DeleteOnSubmit(instance);
                Db.Galleries.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}