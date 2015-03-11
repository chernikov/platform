using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<FailedMail> FailedMails
        {
            get
            {
                return Db.FailedMails;
            }
        }

        public bool CreateFailedMail(FailedMail instance)
        {
            if (instance.ID == 0)
            {
                Db.FailedMails.InsertOnSubmit(instance);
                Db.FailedMails.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public FailedMail PopFailedMail()
        {
            var cache = Db.FailedMails.FirstOrDefault(p => !p.IsProcessed);
            if (cache != null)
            {
                cache.IsProcessed = true;
                Db.FailedMails.Context.SubmitChanges();
                return cache;
            }
            return null;
        }

        public bool RemoveFailedMail(int idFailedMail)
        {
            FailedMail instance = Db.FailedMails.FirstOrDefault(p => p.ID == idFailedMail);
            if (instance != null)
            {
                Db.FailedMails.DeleteOnSubmit(instance);
                Db.FailedMails.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}