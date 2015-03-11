using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PeopleSaying> PeopleSayings
        {
            get
            {
                return Db.PeopleSayings;
            }
        }

        public bool CreatePeopleSaying(PeopleSaying instance)
        {
            if (instance.ID == 0)
            {
                Db.PeopleSayings.InsertOnSubmit(instance);
                Db.PeopleSayings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePeopleSaying(PeopleSaying instance)
        {
            var cache = Db.PeopleSayings.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Author = instance.Author;
				cache.ImagePath = instance.ImagePath;
				cache.Text = instance.Text;
                Db.PeopleSayings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePeopleSaying(int idPeopleSaying)
        {
            PeopleSaying instance = Db.PeopleSayings.FirstOrDefault(p => p.ID == idPeopleSaying);
            if (instance != null)
            {
                Db.PeopleSayings.DeleteOnSubmit(instance);
                Db.PeopleSayings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}