using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Page> Pages
        {
            get
            {
                return Db.Pages;
            }
        }

        public bool CreatePage(Page instance)
        {
            if (instance.ID == 0)
            {
                Db.Pages.InsertOnSubmit(instance);
                Db.Pages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePage(Page instance)
        {
            var cache = Db.Pages.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.Text = instance.Text;
                Db.Pages.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePage(int idPage)
        {
            Page instance = Db.Pages.FirstOrDefault(p => p.ID == idPage);
            if (instance != null)
            {
                Db.Pages.DeleteOnSubmit(instance);
                Db.Pages.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}