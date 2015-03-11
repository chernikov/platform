using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<State> States
        {
            get
            {
                return Db.States;
            }
        }

        public bool CreateState(State instance)
        {
            if (instance.ID == 0)
            {
                Db.States.InsertOnSubmit(instance);
                Db.States.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateState(State instance)
        {
            var cache = Db.States.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Code = instance.Code;
                Db.States.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveState(int idState)
        {
            State instance = Db.States.FirstOrDefault(p => p.ID == idState);
            if (instance != null)
            {
                Db.States.DeleteOnSubmit(instance);
                Db.States.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}