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

        private DateTime? currentDateTime;

        public DateTime CurrentDateTime
        {
            get
            {
                var source = DateTime.Now;
                if (!currentDateTime.HasValue)
                {
                    var setting = Db.Settings.FirstOrDefault(p => p.Name == "CurrentDate");
                    if (setting != null)
                    {
                        currentDateTime = DateTime.Parse(setting.Value, CultureInfo.InvariantCulture).Date;
                    } else {
                        currentDateTime = source.Date;
                    }
                }
                return currentDateTime.Value.AddHours(source.Hour).AddMinutes(source.Minute).AddSeconds(source.Second);
                
            }
        }
    }
}