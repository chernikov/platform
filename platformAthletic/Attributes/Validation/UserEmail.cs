using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;


namespace platformAthletic.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UserEmailValidationAttribute : UniqueValidationAttribute
    {
        protected override bool CheckProperty(object obj)
        {
            if (!(obj is BaseUserView))
                return true;
            var userView = obj as BaseUserView;
            return repository.Users.Count(p => string.Compare(p.Email, userView.Email, true) == 0 && p.ID != userView.ID) == 0;
        }
    }
}