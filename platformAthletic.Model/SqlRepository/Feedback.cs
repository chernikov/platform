using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Feedback> Feedbacks
        {
            get
            {
                return Db.Feedbacks;
            }
        }

        public bool CreateFeedback(Feedback instance)
        {
            if (instance.ID == 0)
            {
                instance.AddedDate = DateTime.Now;
                Db.Feedbacks.InsertOnSubmit(instance);
                Db.Feedbacks.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool ReadFeedback(Feedback instance)
        {
            var cache = Db.Feedbacks.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.IsReaded = true;
                Db.Feedbacks.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFeedback(int idFeedback)
        {
            Feedback instance = Db.Feedbacks.FirstOrDefault(p => p.ID == idFeedback);
            if (instance != null)
            {
                Db.Feedbacks.DeleteOnSubmit(instance);
                Db.Feedbacks.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}