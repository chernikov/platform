using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<TrainingSet> TrainingSets
        {
            get
            {
                return Db.TrainingSets;
            }
        }

        public bool CreateTrainingSet(TrainingSet instance)
        {
            if (instance.ID == 0)
            {
                Db.TrainingSets.InsertOnSubmit(instance);
                Db.TrainingSets.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTrainingSet(TrainingSet instance)
        {
            var cache = Db.TrainingSets.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.DayID = instance.DayID;
                cache.PhaseID = instance.PhaseID;
                cache.TrainingEquipments = instance.TrainingEquipments;
                Db.TrainingSets.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTrainingSet(int idTrainingSet)
        {
            TrainingSet instance = Db.TrainingSets.FirstOrDefault(p => p.ID == idTrainingSet);
            if (instance != null)
            {
                Db.TrainingSets.DeleteOnSubmit(instance);
                Db.TrainingSets.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}