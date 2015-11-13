using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace platformAthletic.Model
{
    public partial class SqlRepository
    {
        public IQueryable<DelayedJob> DelayedJobs
        {
            get
            {
                return Db.DelayedJobs;
            }
        }

        public bool CreateDelayedJob(DelayedJob instance)
        {
            if (instance.ID == 0)
            {
                Db.DelayedJobs.InsertOnSubmit(instance);
                Db.DelayedJobs.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveDelayedJob(int idDelayedJob)
        {
            DelayedJob insance = Db.DelayedJobs.FirstOrDefault(p => p.ID == idDelayedJob);
            if (insance != null)
            {
                Db.DelayedJobs.DeleteOnSubmit(insance);
                Db.DelayedJobs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ChangeDelayedJobStatus(int idDelayedJob, string status)
        {
            DelayedJob insance = Db.DelayedJobs.FirstOrDefault(p => p.ID == idDelayedJob);
            if (insance != null)
            {
                insance.Status = status;
                Db.DelayedJobs.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}
