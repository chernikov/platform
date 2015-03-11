using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Cycle> Cycles
        {
            get
            {
                return Db.Cycles;
            }
        }

        public bool CreateCycle(Cycle instance)
        {
            if (instance.ID == 0)
            {
                Db.Cycles.InsertOnSubmit(instance);
                Db.Cycles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCycle(Cycle instance)
        {
            var cache = Db.Cycles.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.SeasonID = instance.SeasonID;
				cache.Name = instance.Name;
                Db.Cycles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCycle(int idCycle)
        {
            Cycle instance = Db.Cycles.FirstOrDefault(p => p.ID == idCycle);
            if (instance != null)
            {
                Db.Cycles.DeleteOnSubmit(instance);
                Db.Cycles.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}