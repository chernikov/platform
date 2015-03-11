using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<PagePart> PageParts
        {
            get
            {
                return Db.PageParts;
            }
        }

        public bool SavePagePart(PagePart instance)
        {
            var cache = Db.PageParts.FirstOrDefault(p =>
                string.Compare(p.Name, instance.Name, true) == 0);
            if (cache != null)
            {
                cache.Text = instance.Text;
            }
            else
            {
                Db.PageParts.InsertOnSubmit(instance);
            }
            Db.PageParts.Context.SubmitChanges();
            return true;
        }
    }
}