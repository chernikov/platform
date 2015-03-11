using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TrainingDayCell> TrainingDayCells
        {
            get
            {
                return Db.TrainingDayCells;
            }
        }

        public bool CreateTrainingDayCell(TrainingDayCell instance)
        {
            if (instance.ID == 0)
            {
                Db.TrainingDayCells.InsertOnSubmit(instance);
                Db.TrainingDayCells.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTrainingDayCell(TrainingDayCell instance)
        {
            var cache = Db.TrainingDayCells.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.CellID = instance.CellID;
				cache.TrainingDayID = instance.TrainingDayID;
				cache.PrimaryText = instance.PrimaryText;
				cache.TrainingSetID = instance.TrainingSetID;
				cache.SBCType = instance.SBCType;
				cache.Coefficient = instance.Coefficient;
                Db.TrainingDayCells.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTrainingDayCell(int idTrainingDayCell)
        {
            TrainingDayCell instance = Db.TrainingDayCells.FirstOrDefault(p => p.ID == idTrainingDayCell);
            if (instance != null)
            {
                Db.TrainingDayCells.DeleteOnSubmit(instance);
                Db.TrainingDayCells.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}