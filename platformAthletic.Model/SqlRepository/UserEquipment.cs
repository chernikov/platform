using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserEquipment> UserEquipments
        {
            get
            {
                return Db.UserEquipments;
            }
        }

        public bool CreateUserEquipment(UserEquipment instance)
        {
            if (instance.ID == 0)
            {
                Db.UserEquipments.InsertOnSubmit(instance);
                Db.UserEquipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserEquipment(UserEquipment instance)
        {
            var cache = Db.UserEquipments.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.UserID = instance.UserID;
				cache.EquipmentID = instance.EquipmentID;
                Db.UserEquipments.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserEquipment(int idUserEquipment)
        {
            UserEquipment instance = Db.UserEquipments.FirstOrDefault(p => p.ID == idUserEquipment);
            if (instance != null)
            {
                Db.UserEquipments.DeleteOnSubmit(instance);
                Db.UserEquipments.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}