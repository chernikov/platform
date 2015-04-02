using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        

        public IQueryable<Level> Levels
        {
            get
            {
                return Db.Levels;
            }
        }

        public bool CreateLevel(Level instance)
        {
            if (instance.ID == 0)
            {
                Db.Levels.InsertOnSubmit(instance);
                Db.Levels.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateLevel(Level instance)
        {
            var cache = Db.Levels.Where(p => p.ID == instance.ID).FirstOrDefault();
            if (cache != null)
            {
                cache.Name = instance.Name;
                Db.Levels.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveLevel(int idLevel)
        {
            Level instance = Db.Levels.Where(p => p.ID == idLevel).FirstOrDefault();
            if (instance != null)
            {
                Db.Levels.DeleteOnSubmit(instance);
                Db.Levels.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        
    }
}
