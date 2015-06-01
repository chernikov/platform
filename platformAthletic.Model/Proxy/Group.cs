using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Group
    {
        public IEnumerable<User> SubUsers
        {
            get
            {
                return Users.Where(p => !p.IsDeleted).OrderBy(p => p.LastName).AsEnumerable();
            }
        }
	}
}