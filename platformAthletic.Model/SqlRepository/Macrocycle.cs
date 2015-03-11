using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Macrocycle> Macrocycles
        {
            get
            {
                return Db.Macrocycles;
            }
        }

        public bool CreateMacrocycle(Macrocycle instance)
        {
            if (instance.ID == 0)
            {
                Db.Macrocycles.InsertOnSubmit(instance);
                Db.Macrocycles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMacrocycle(Macrocycle instance)
        {
            var cache = Db.Macrocycles.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.WeekID = instance.WeekID;
				cache.Name = instance.Name;
                Db.Macrocycles.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateMacrocycleName(Macrocycle instance)
        {
            var cache = Db.Macrocycles.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Name = instance.Name;
                Db.Macrocycles.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RemoveMacrocycle(int idMacrocycle)
        {
            Macrocycle instance = Db.Macrocycles.FirstOrDefault(p => p.ID == idMacrocycle);
            if (instance != null)
            {
                Db.Macrocycles.DeleteOnSubmit(instance);
                Db.Macrocycles.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}