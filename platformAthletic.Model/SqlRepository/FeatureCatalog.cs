using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<FeatureCatalog> FeatureCatalogs
        {
            get
            {
                return Db.FeatureCatalogs;
            }
        }

        public bool CreateFeatureCatalog(FeatureCatalog instance)
        {
            if (instance.ID == 0)
            {
                var lastOrderBy = FeatureCatalogs.OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                instance.OrderBy = lastOrderBy + 1;
                Db.FeatureCatalogs.InsertOnSubmit(instance);
                Db.FeatureCatalogs.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateFeatureCatalog(FeatureCatalog instance)
        {
            var cache = Db.FeatureCatalogs.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Header = instance.Header;
                cache.OrderBy = instance.OrderBy;
                Db.FeatureCatalogs.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFeatureCatalog(int idFeatureCatalog)
        {
            FeatureCatalog instance = Db.FeatureCatalogs.FirstOrDefault(p => p.ID == idFeatureCatalog);
            if (instance != null)
            {
                foreach (var forFeature in FeatureCatalogs.Where(p => p.OrderBy > instance.OrderBy))
                {
                    forFeature.OrderBy--;
                }
                
                Db.FeatureCatalogs.DeleteOnSubmit(instance);
                Db.FeatureCatalogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

       
        public bool MoveFeatureCatalog(int id, int placeBefore)
        {
            var FeatureCatalog = Db.FeatureCatalogs.FirstOrDefault(p => p.ID == id);
            if (FeatureCatalog != null)
            {
                if (FeatureCatalog.OrderBy > placeBefore)
                {
                    foreach (var forFeatureCatalog in FeatureCatalogs.Where(w => w.OrderBy >= placeBefore && w.OrderBy < FeatureCatalog.OrderBy))
                    {
                        forFeatureCatalog.OrderBy++;
                    }
                }

                if (FeatureCatalog.OrderBy < placeBefore)
                {
                    foreach (var forCatalog in FeatureCatalogs.Where(w => w.OrderBy > FeatureCatalog.OrderBy && w.OrderBy <= placeBefore))
                    {
                        forCatalog.OrderBy--;
                    }
                }
                FeatureCatalog.OrderBy = placeBefore;
                Db.FeatureCatalogs.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}