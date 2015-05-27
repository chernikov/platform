using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Team> Teams
        {
            get
            {
                return Db.Teams;
            }
        }

        public bool CreateTeam(Team instance)
        {
            if (instance.ID == 0)
            {
                instance.PrimaryColor = "#4E1305";
                instance.SecondaryColor = "#000000";
                instance.MaxCount = 100;
                Db.Teams.InsertOnSubmit(instance);
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTeam(Team instance)
        {
            var cache = Db.Teams.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.Name = instance.Name;
				cache.LogoPath = instance.LogoPath;
				cache.StateID = instance.StateID;
                cache.SchoolID = instance.SchoolID;
                cache.SBCControl = instance.SBCControl;
				cache.PrimaryColor = instance.PrimaryColor ?? "#ffffff";
                cache.SecondaryColor = instance.SecondaryColor ?? "#000000";
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateSettingTeam(Team instance)
        {
            var cache = Db.Teams.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.LogoPath = instance.LogoPath;
                cache.SBCControl = instance.SBCControl;
                cache.SBCAttendance = instance.SBCAttendance;
                cache.PrimaryColor = instance.PrimaryColor ?? "#ffffff";
                cache.SecondaryColor = instance.SecondaryColor ?? "#000000";
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTeamCount(Team instance)
        {
            var cache = Db.Teams.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.MaxCount = instance.MaxCount;
                Db.Teams.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTeam(int idTeam)
        {
            Team instance = Db.Teams.FirstOrDefault(p => p.ID == idTeam);
            if (instance != null)
            {
                foreach (var sbcValue in instance.SBCValues.ToList())
                {
                    sbcValue.TeamID = null;
                }
                Db.SBCValues.Context.SubmitChanges();

                var scheduleList = instance.Schedules.ToList();
                Db.Schedules.DeleteAllOnSubmit(scheduleList);


                Db.Teams.DeleteOnSubmit(instance);
                Db.Teams.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}