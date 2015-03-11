using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserAttendance> UserAttendances
        {
            get
            {
                return Db.UserAttendances;
            }
        }

        public bool CreateUserAttendance(UserAttendance instance)
        {
            if (instance.ID == 0)
            {
                Db.UserAttendances.InsertOnSubmit(instance);
                Db.UserAttendances.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserAttendance(int idUserAttendance)
        {
            UserAttendance instance = Db.UserAttendances.FirstOrDefault(p => p.ID == idUserAttendance);
            if (instance != null)
            {
                Db.UserAttendances.DeleteOnSubmit(instance);
                Db.UserAttendances.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}