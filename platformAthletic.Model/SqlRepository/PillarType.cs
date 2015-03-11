using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PillarType> PillarTypes
        {
            get
            {
                return Db.PillarTypes;
            }
        }

        public bool CreatePillarType(PillarType instance)
        {
            if (instance.ID == 0)
            {
                Db.PillarTypes.InsertOnSubmit(instance);
                Db.PillarTypes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdatePillarType(PillarType instance)
        {
            var cache = Db.PillarTypes.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Type = instance.Type;
                cache.Placeholder = instance.Placeholder;
				cache.Name = instance.Name;
                cache.Type = instance.Type;
				cache.Measure = instance.Measure;
                cache.TextAbove = instance.TextAbove;
                cache.VideoCode = instance.VideoCode;
                cache.VideoUrl = instance.VideoUrl;
                Db.PillarTypes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemovePillarType(int idPillarType)
        {
            PillarType instance = Db.PillarTypes.FirstOrDefault(p => p.ID == idPillarType);
            if (instance != null)
            {
                Db.PillarTypes.DeleteOnSubmit(instance);
                Db.PillarTypes.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}