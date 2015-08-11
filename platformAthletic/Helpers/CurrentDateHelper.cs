using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Helpers
{
    public static class CurrentDateHelper
    {
        public static DateTime Current(this DateTime source)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            return repository.CurrentDateTime;
            return source;
        }
    }
}