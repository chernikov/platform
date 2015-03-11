using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class Post
    {
        public bool CanEdit(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (user.ID == UserID || user.InRoles("admin"))
            {
                return true;
            }
            return false;
                
        }
	}
}