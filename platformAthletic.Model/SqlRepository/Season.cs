using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Season> Seasons
        {
            get
            {
                return Db.Seasons;
            }
        }

        public bool CreateSeason(Season instance)
        {
            if (instance.ID == 0)
            {
                Db.Seasons.InsertOnSubmit(instance);
                Db.Seasons.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSeason(Season instance)
        {
            var cache = Db.Seasons.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Type = instance.Type;
				cache.Name = instance.Name;
                Db.Seasons.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSeason(int idSeason)
        {
            Season instance = Db.Seasons.FirstOrDefault(p => p.ID == idSeason);
            if (instance != null)
            {
                Db.Seasons.DeleteOnSubmit(instance);
                Db.Seasons.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}