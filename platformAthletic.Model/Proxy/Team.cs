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

        public UserSeason CurrentSeason
        {
            get
            {
                return User.CurrentSeason;
            }
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
                return Users.Count();
            }
        }

        public IEnumerable<User> SubUngroupUsers
        {
            get
            {
                return Users.Where(p => !p.GroupID.HasValue).OrderBy(p => p.LastName).AsEnumerable();
            }
        }


        public IEnumerable<User> SubUsers
        {
            get
            {
                return Users.OrderBy(p => p.LastName).AsEnumerable();
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