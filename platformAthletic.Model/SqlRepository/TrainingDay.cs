using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TrainingDay> TrainingDays
        {
            get
            {
                return Db.TrainingDays;
            }
        }

        public bool CreateTrainingDay(TrainingDay instance)
        {
            if (instance.ID == 0)
            {
                Db.TrainingDays.InsertOnSubmit(instance);
                Db.TrainingDays.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTrainingDay(TrainingDay instance)
        {
            var cache = Db.TrainingDays.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.WeekID = instance.WeekID;
                cache.DayID = instance.DayID;
                Db.TrainingDays.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTrainingDay(int idTrainingDay)
        {
            TrainingDay instance = Db.TrainingDays.FirstOrDefault(p => p.ID == idTrainingDay);
            if (instance != null)
            {
                Db.TrainingDays.DeleteOnSubmit(instance);
                Db.TrainingDays.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}