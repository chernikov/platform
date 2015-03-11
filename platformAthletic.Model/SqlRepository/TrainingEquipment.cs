using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TrainingEquipment> TrainingEquipments
        {
            get
            {
                return Db.TrainingEquipments;
            }
        }

        public bool CreateTrainingEquipment(TrainingEquipment instance)
        {
            if (instance.ID == 0)
            {
                Db.TrainingEquipments.InsertOnSubmit(instance);
                Db.TrainingEquipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTrainingEquipment(TrainingEquipment instance)
        {
            var cache = Db.TrainingEquipments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TrainingSetID = instance.TrainingSetID;
				cache.TrainingID = instance.TrainingID;
				cache.EquipmentID = instance.EquipmentID;
				cache.Priority = instance.Priority;
                Db.TrainingEquipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTrainingEquipment(int idTrainingEquipment)
        {
            TrainingEquipment instance = Db.TrainingEquipments.FirstOrDefault(p => p.ID == idTrainingEquipment);
            if (instance != null)
            {
                Db.TrainingEquipments.DeleteOnSubmit(instance);
                Db.TrainingEquipments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}