using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Training> Trainings
        {
            get
            {
                return Db.Trainings;
            }
        }

        public bool CreateTraining(Training instance)
        {
            if (instance.ID == 0)
            {
                Db.Trainings.InsertOnSubmit(instance);
                Db.Trainings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTraining(Training instance)
        {
            var cache = Db.Trainings.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Trainings.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTraining(int idTraining)
        {
            Training instance = Db.Trainings.FirstOrDefault(p => p.ID == idTraining);
            if (instance != null)
            {
                Db.Trainings.DeleteOnSubmit(instance);
                Db.Trainings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}