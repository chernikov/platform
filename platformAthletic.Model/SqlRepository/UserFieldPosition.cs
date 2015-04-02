using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        public IQueryable<UserFieldPosition> UserFieldPositions
        {
            get
            {
                return Db.UserFieldPositions;
            }
        }

        public bool CreateUserFieldPosition(UserFieldPosition instance)
        {
            if (instance.ID == 0)
            {
                Db.UserFieldPositions.InsertOnSubmit(instance);
                Db.UserFieldPositions.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserFieldPosition(int idUserFieldPosition)
        {
            UserFieldPosition instance = Db.UserFieldPositions.Where(p => p.ID == idUserFieldPosition).FirstOrDefault();
            if (instance != null)
            {
                Db.UserFieldPositions.DeleteOnSubmit(instance);
                Db.UserFieldPositions.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        
    }
}
