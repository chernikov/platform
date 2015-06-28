using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Group> Groups
        {
            get
            {
                return Db.Groups;
            }
        }

        public bool CreateGroup(Group instance)
        {
            if (instance.ID == 0)
            {
                Db.Groups.InsertOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateGroup(Group instance)
        {
            var cache = Db.Groups.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
                Db.Groups.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveGroup(int idGroup)
        {
            Group instance = Db.Groups.FirstOrDefault(p => p.ID == idGroup);
            if (instance != null)
            {
                var users = instance.Users.ToList();
                foreach (var user in users)
                {
                    var cache = Db.Users.FirstOrDefault(p => p.ID == user.ID);
                    if (cache != null)
                    {
                        cache.GroupID = null;
                        Db.Users.Context.SubmitChanges();
                    }
                }
                var schedules = Db.Schedules.Where(p => p.GroupID == instance.ID);
                Db.Schedules.DeleteAllOnSubmit(schedules);
                Db.Schedules.Context.SubmitChanges();
                var userSeasons = Db.UserSeasons.Where(p => p.GroupID == instance.ID);
                Db.UserSeasons.DeleteAllOnSubmit(userSeasons);
                Db.UserSeasons.Context.SubmitChanges();
                Db.Groups.DeleteOnSubmit(instance);
                Db.Groups.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}