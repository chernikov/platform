using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Setting> Settings
        {
            get
            {
                return Db.Settings;
            }
        }

        public void SaveSetting(Setting instance)
        {
            var cache = Db.Settings.FirstOrDefault(p => p.Name == instance.Name);
            if (cache != null)
            {
                cache.Value = instance.Value;
                Db.Settings.Context.SubmitChanges();
            }
            else
            {
                Db.Settings.InsertOnSubmit(instance);
                Db.Settings.Context.SubmitChanges();
            }
        }

        public bool RemoveSetting(int idSetting)
        {
            Setting instance = Db.Settings.FirstOrDefault(p => p.ID == idSetting);
            if (instance != null)
            {
                Db.Settings.DeleteOnSubmit(instance);
                Db.Settings.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}