using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Day> Days
        {
            get
            {
                return Db.Days;
            }
        }

        public bool CreateDay(Day instance)
        {
            if (instance.ID == 0)
            {
                Db.Days.InsertOnSubmit(instance);
                Db.Days.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateDay(Day instance)
        {
            var cache = Db.Days.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Days.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveDay(int idDay)
        {
            Day instance = Db.Days.FirstOrDefault(p => p.ID == idDay);
            if (instance != null)
            {
                Db.Days.DeleteOnSubmit(instance);
                Db.Days.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}