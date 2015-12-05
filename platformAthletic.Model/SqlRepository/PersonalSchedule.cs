using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PersonalSchedule> PersonalSchedules
        {
            get
            {
                return Db.PersonalSchedules;
            }
        }

        public bool CreatePersonalSchedule(PersonalSchedule instance)
        {
            if (instance.ID == 0)
            {
                PersonalSchedule exist = null;
                exist = Db.PersonalSchedules.FirstOrDefault(p => p.Number == instance.Number 
                    && p.UserSeasonID == instance.UserSeasonID 
                    && p.UserID == instance.UserID);
                
                if (exist != null)
                {
                    Db.PersonalSchedules.DeleteOnSubmit(exist);
                    Db.PersonalSchedules.Context.SubmitChanges();
                }
                Db.PersonalSchedules.InsertOnSubmit(instance);
                Db.PersonalSchedules.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool UpdatePersonalSchedule(PersonalSchedule instance)
        {
            var cache = Db.PersonalSchedules.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.UserSeasonID = instance.UserSeasonID;
				cache.UserID = instance.UserID;
				cache.Number = instance.Number;
				cache.MacrocycleID = instance.MacrocycleID;
                cache.Date = instance.Date;
                Db.PersonalSchedules.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePersonalSchedule(int idPersonalSchedule)
        {
            PersonalSchedule instance = Db.PersonalSchedules.FirstOrDefault(p => p.ID == idPersonalSchedule);
            if (instance != null)
            {
                Db.PersonalSchedules.DeleteOnSubmit(instance);
                Db.PersonalSchedules.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ResetPersonalSchedule(int idUser)
        {
            var schedules = Db.PersonalSchedules.Where(p => p.UserID == idUser).ToList();
            Db.PersonalSchedules.DeleteAllOnSubmit(schedules);
            Db.PersonalSchedules.Context.SubmitChanges();
            return true;
        }
    }
}