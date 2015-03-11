using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Equipment> Equipments
        {
            get
            {
                return Db.Equipments;
            }
        }

        public bool CreateEquipment(Equipment instance)
        {
            if (instance.ID == 0)
            {
                Db.Equipments.InsertOnSubmit(instance);
                Db.Equipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateEquipment(Equipment instance)
        {
            var cache = Db.Equipments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.ImagePath = instance.ImagePath;
                Db.Equipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveEquipment(int idEquipment)
        {
            Equipment instance = Db.Equipments.FirstOrDefault(p => p.ID == idEquipment);
            if (instance != null)
            {
                Db.Equipments.DeleteOnSubmit(instance);
                Db.Equipments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}