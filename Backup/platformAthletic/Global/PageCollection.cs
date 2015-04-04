using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using platformAthletic.Model;

namespace platformAthletic.Global
{
    public class PageCollection
    {
        [Inject]
        public IRepository Repository { get; set; }

        public string this[string index]
        {
            get
            {
                var page = Repository.Pages.FirstOrDefault(p => string.Compare(p.Name, index, true) == 0);
                if (page != null) 
                {
                    return page.Text;
                }
                return string.Empty;
            }
        }
    }
}