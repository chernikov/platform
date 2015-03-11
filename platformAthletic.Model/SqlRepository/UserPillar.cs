using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserPillar> UserPillars
        {
            get
            {
                return Db.UserPillars;
            }
        }

        public bool CreateUserPillar(UserPillar instance)
        {
            if (instance.ID == 0)
            {
                Db.UserPillars.InsertOnSubmit(instance);
                Db.UserPillars.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserPillar(int idUserPillar)
        {
            UserPillar instance = Db.UserPillars.FirstOrDefault(p => p.ID == idUserPillar);
            if (instance != null)
            {
                Db.UserPillars.DeleteOnSubmit(instance);
                Db.UserPillars.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}