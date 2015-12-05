using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Schedule> Schedules
        {
            get
            {
                return Db.Schedules;
            }
        }

        public bool CreateSchedule(Schedule instance)
        {
            if (instance.ID == 0)
            {
                Schedule exist = null;
                if (instance.GroupID != null)
                {
                    exist = Db.Schedules.FirstOrDefault(p => p.Number == instance.Number 
                        && p.GroupID == instance.GroupID 
                        && p.TeamID == instance.TeamID);
                }
                else
                {
                    exist = Db.Schedules.FirstOrDefault(p => p.Number == instance.Number 
                        && p.GroupID == null 
                        && p.TeamID == instance.TeamID);
                }

                if (exist != null)
                {
                    Db.Schedules.DeleteOnSubmit(exist);
                    Db.Schedules.Context.SubmitChanges();
                }
                Db.Schedules.InsertOnSubmit(instance);
                Db.Schedules.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSchedule(Schedule instance)
        {
            var cache = Db.Schedules.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TeamID = instance.TeamID;
				cache.GroupID = instance.GroupID;
                cache.UserSeasonID = instance.UserSeasonID;
				cache.Number = instance.Number;
				cache.MacrocycleID = instance.MacrocycleID;
                cache.Date = instance.Date;
                Db.Schedules.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSchedule(int idSchedule)
        {
            var instance = Db.Schedules.FirstOrDefault(p => p.ID == idSchedule);
            if (instance != null)
            {
                Db.Schedules.DeleteOnSubmit(instance);
                Db.Schedules.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ResetSchedule(int idTeam, int? groupID)
        {
            if (groupID.HasValue)
            {
                var schedules = Db.Schedules.Where(p => p.TeamID == idTeam && p.GroupID == groupID.Value).ToList();
                Db.Schedules.DeleteAllOnSubmit(schedules);
                Db.Schedules.Context.SubmitChanges();
            }
            else
            {
                var schedules = Db.Schedules.Where(p => p.TeamID == idTeam && p.GroupID == null).ToList();
                Db.Schedules.DeleteAllOnSubmit(schedules);
                Db.Schedules.Context.SubmitChanges();
            }
            return true;
        }
            
    }
}