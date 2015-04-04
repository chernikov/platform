using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        

        public IQueryable<School> Schools
        {
            get
            {
                return Db.Schools;
            }
        }

        public bool CreateSchool(School instance)
        {
            if (instance.ID == 0)
            {
                Db.Schools.InsertOnSubmit(instance);
                Db.Schools.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSchool(School instance)
        {
            var cache = Db.Schools.Where(p => p.ID == instance.ID).FirstOrDefault();
            if (cache != null)
            {
                //TODO : Update fields for School
                Db.Schools.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSchool(int idSchool)
        {
            var instance = Db.Schools.Where(p => p.ID == idSchool).FirstOrDefault();
            if (instance != null)
            {
                Db.Schools.DeleteOnSubmit(instance);
                Db.Schools.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        
    }
}
