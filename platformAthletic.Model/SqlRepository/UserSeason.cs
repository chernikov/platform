using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<UserSeason> UserSeasons
        {
            get
            {
                return Db.UserSeasons;
            }
        }

        public bool CreateUserSeason(UserSeason instance)
        {
            if (instance.ID == 0)
            {
                Db.UserSeasons.InsertOnSubmit(instance);
                Db.UserSeasons.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateUserSeason(UserSeason instance)
        {
            var cache = Db.UserSeasons.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.SeasonID = instance.SeasonID;
				cache.StartDay = instance.StartDay;
                Db.UserSeasons.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserSeason(int idUserSeason)
        {
            UserSeason instance = Db.UserSeasons.FirstOrDefault(p => p.ID == idUserSeason);
            if (instance != null)
            {
                Db.UserSeasons.DeleteOnSubmit(instance);
                Db.UserSeasons.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}