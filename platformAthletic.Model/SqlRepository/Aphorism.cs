using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Aphorism> Aphorisms
        {
            get
            {
                return Db.Aphorisms;
            }
        }

        public bool CreateAphorism(Aphorism instance)
        {
            if (instance.ID == 0)
            {
                Db.Aphorisms.InsertOnSubmit(instance);
                Db.Aphorisms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateAphorism(Aphorism instance)
        {
            var cache = Db.Aphorisms.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Author = instance.Author;
				cache.Text = instance.Text;
                Db.Aphorisms.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveAphorism(int idAphorism)
        {
            Aphorism instance = Db.Aphorisms.FirstOrDefault(p => p.ID == idAphorism);
            if (instance != null)
            {
                Db.Aphorisms.DeleteOnSubmit(instance);
                Db.Aphorisms.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}