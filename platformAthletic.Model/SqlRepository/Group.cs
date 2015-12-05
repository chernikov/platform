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
                var userSeasons = Db.UserSeasons.Where(p => p.GroupID == null && p.UserID == instance.Team.UserID).ToList();
                foreach (var userSeason in userSeasons)
                {
                    var newUserSeason = new UserSeason()
                    {
                        SeasonID = userSeason.SeasonID,
                        UserID = userSeason.UserID,
                        GroupID = instance.ID,
                        StartDay = userSeason.StartDay,
                        StartFrom = userSeason.StartFrom
                    };
                    Db.UserSeasons.InsertOnSubmit(newUserSeason);
                    Db.UserSeasons.Context.SubmitChanges();
                    var schedules = Db.Schedules.Where(p => p.GroupID == null && p.TeamID == instance.TeamID && p.UserSeasonID == userSeason.ID).ToList();
                    foreach (var schedule in schedules)
                    {
                        var newSchedule = new Schedule()
                        {
                            UserSeasonID = newUserSeason.ID,
                            TeamID = schedule.TeamID,
                            GroupID = instance.ID,
                            Number = schedule.Number,
                            MacrocycleID = schedule.MacrocycleID,
                            Date = schedule.Date
                        };
                        Db.Schedules.InsertOnSubmit(newSchedule);
                    }
                    Db.Schedules.Context.SubmitChanges();
                }
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