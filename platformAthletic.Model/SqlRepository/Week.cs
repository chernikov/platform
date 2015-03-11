using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Week> Weeks
        {
            get
            {
                return Db.Weeks;
            }
        }

        public bool CreateWeek(Week instance)
        {
            if (instance.ID == 0)
            {
                Db.Weeks.InsertOnSubmit(instance);
                Db.Weeks.Context.SubmitChanges();

                CreateMacrocycle(new Macrocycle()
                {
                    Name = instance.FullName,
                    WeekID = instance.ID
                });
                return true;
            }

            return false;
        }

        public bool UpdateWeek(Week instance)
        {
            var cache = Db.Weeks.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.PhaseID = instance.PhaseID;
				cache.Name = instance.Name;
                cache.Number = instance.Number;
                Db.Weeks.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveWeek(int idWeek)
        {
            var instance = Db.Weeks.FirstOrDefault(p => p.ID == idWeek);
            if (instance != null)
            {
                var list = Db.Macrocycles.Where(p => p.WeekID == instance.ID).ToList();
                Db.Macrocycles.DeleteAllOnSubmit(list);
                Db.Weeks.DeleteOnSubmit(instance);
                Db.Weeks.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}