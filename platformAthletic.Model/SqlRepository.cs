using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Ninject;

namespace platformAthletic.Model
{
    public partial class SqlRepository : IRepository
    {
        [Inject]
        public platformAthleticDbDataContext Db { get; set; }

        public IQueryable<T> GetTable<T>() where T : class
        {
            return Db.GetTable<T>().AsQueryable<T>();
        }
    }
}