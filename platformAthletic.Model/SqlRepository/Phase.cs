using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Phase> Phases
        {
            get
            {
                return Db.Phases;
            }
        }

        public bool CreatePhase(Phase instance)
        {
            if (instance.ID == 0)
            {
                Db.Phases.InsertOnSubmit(instance);
                Db.Phases.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePhase(Phase instance)
        {
            var cache = Db.Phases.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.CycleID = instance.CycleID;
				cache.Name = instance.Name;
                Db.Phases.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePhase(int idPhase)
        {
            Phase instance = Db.Phases.FirstOrDefault(p => p.ID == idPhase);
            if (instance != null)
            {
                Db.Phases.DeleteOnSubmit(instance);
                Db.Phases.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}