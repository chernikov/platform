using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<FieldPosition> FieldPositions
        {
            get
            {
                return Db.FieldPositions;
            }
        }

        public bool CreateFieldPosition(FieldPosition instance)
        {
            if (instance.ID == 0)
            {
                Db.FieldPositions.InsertOnSubmit(instance);
                Db.FieldPositions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateFieldPosition(FieldPosition instance)
        {
            var cache = Db.FieldPositions.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Code = instance.Code;
				cache.Name = instance.Name;
                Db.FieldPositions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFieldPosition(int idFieldPosition)
        {
            FieldPosition instance = Db.FieldPositions.FirstOrDefault(p => p.ID == idFieldPosition);
            if (instance != null)
            {
                Db.FieldPositions.DeleteOnSubmit(instance);
                Db.FieldPositions.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}