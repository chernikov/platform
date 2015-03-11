using System.Linq;
using platformAthletic.Controllers;
using platformAthletic.Model;
using System.Web.Routing;
using System.Globalization;
using System.Threading;

namespace platformAthletic.Areas.Admin.Controllers
{
    public abstract class AdminController : BaseController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

    }
}
