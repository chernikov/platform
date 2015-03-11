using System;
using System.Linq;
using platformAthletic.Models.ViewModels.User;

namespace platformAthletic.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsUserPasswordAttribute : UniqueValidationAttribute
    {
        protected override bool CheckProperty(object obj)
        {
            if (!(obj is ChangePasswordView))
            {
                return true;
            }

            var changePasswordView = obj as ChangePasswordView;
            return repository.Users.Count(p => p.ID == changePasswordView.ID && p.Password == changePasswordView.Password) > 0;
        }
    }
}