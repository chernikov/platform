using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Team
    {

        public enum SBCControlType : int
        {
            Coach = 0x00,
            CoachAndPlayer = 0x01
        }

        public enum SBCAttendanceType : int
        {
            Coach = 0x00,
            CoachAndPlayer = 0x01
        }

        public IList<User> Players
        {
            get
            {
                return Users.Where(p => !p.IsDeleted).ToList();
            }
        }

        public IList<User> Assistants
        {
            get
            {
                return Users1;
            }
        }


        public UserSeason GetCurrentSeason(int? groupID = null)
        {
            var coach = this.User;
            if (groupID != null)
            {
                var currentSeason = coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime, groupID);
                if (currentSeason == null)
                {
                    return coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime);
                }
                return currentSeason;
            };
            return coach.SeasonByDateAndGroup(SqlSingleton.sqlRepository.CurrentDateTime);
        }

        public UserSeason NextSeason
        {
            get
            {
                return User.NextSeason;
            }
        }

        public int CountPlayers
        {
            get
            {
                return Players.Count();
            }
        }

        public IEnumerable<User> SubUngroupUsers
        {
            get
            {
                return Players.Where(p => !p.GroupID.HasValue).OrderBy(p => p.LastName).AsEnumerable();
            }
        }

        public IQueryable<User> ActiveUsers
        {
            get
            {
                return Players.Where(p => !p.IsDeleted).AsQueryable();
            }
        }


        public IEnumerable<User> SubUsers
        {
            get
            {
                return Players.OrderBy(p => p.LastName).AsEnumerable();
            }
        }

        public IEnumerable<Group> SubGroups
        {
            get
            {
                return Groups.AsEnumerable();
            }
        }
	}
}