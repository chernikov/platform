using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<About> Abouts
        {
            get
            {
                return Db.Abouts;
            }
        }

        public bool CreateAbout(About instance)
        {
            if (instance.ID == 0)
            {
                Db.Abouts.InsertOnSubmit(instance);
                Db.Abouts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateAbout(About instance)
        {
            var cache = Db.Abouts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Text = instance.Text;
				cache.Author = instance.Author;
                Db.Abouts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveAbout(int idAbout)
        {
            About instance = Db.Abouts.FirstOrDefault(p => p.ID == idAbout);
            if (instance != null)
            {
                Db.Abouts.DeleteOnSubmit(instance);
                Db.Abouts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}