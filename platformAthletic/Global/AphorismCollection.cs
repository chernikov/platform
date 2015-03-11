using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using platformAthletic.Model;

namespace platformAthletic.Global
{
    public class AphorismCollection
    {
        [Inject]
        public IRepository Repository { get; set; }

        public Aphorism Get
        {
            get
            {
                return Repository.Aphorisms.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            }
        }
    }
}