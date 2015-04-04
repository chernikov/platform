using Ninject;
using platformAthletic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Global
{
    public class PagePartProvider
    {
        [Inject]
        public IRepository Repository { get; set; }

        public MvcHtmlString this[string index]
        {
            get
            {
                var item = Repository.PageParts.FirstOrDefault(p => string.Compare(p.Name, index, true) == 0);
                if (item != null)
                {
                    return new MvcHtmlString(item.Text);
                }
                return new MvcHtmlString(string.Empty);
            }
        }
    }
}