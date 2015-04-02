using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        

        public IQueryable<Sport> Sports
        {
            get
            {
                return Db.Sports;
            }
        }

        public bool CreateSport(Sport instance)
        {
            if (instance.ID == 0)
            {
                Db.Sports.InsertOnSubmit(instance);
                Db.Sports.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSport(Sport instance)
        {
            var cache = Db.Sports.Where(p => p.ID == instance.ID).FirstOrDefault();
            if (cache != null)
            {
                cache.Name = instance.Name;
                Db.Sports.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        //TODO: Предупредить сколько это данных заденет
        public bool RemoveSport(int idSport)
        {
            Sport instance = Db.Sports.Where(p => p.ID == idSport).FirstOrDefault();
            if (instance != null)
            {
                Db.Sports.DeleteOnSubmit(instance);
                Db.Sports.Context.SubmitChanges();
                return true;
            }

            return false;
        }
        
    }
}
