using platformAthletic.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Attributes
{
    public class SeasonCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext  filterContext)
        {
            var user = ((BaseController)filterContext.Controller).CurrentUser;

            if (user.CurrentSeason == null)
            {
                filterContext.Result = new RedirectResult("/register-success");
            }
            // The action filter logic.
        }
    }
}