using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<Faq> Faqs
        {
            get
            {
                return Db.Faqs;
            }
        }

        public bool CreateFaq(Faq instance)
        {
            if (instance.ID == 0)
            {
                var lastOrderBy = Faqs.OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                instance.OrderBy = lastOrderBy + 1;
                Db.Faqs.InsertOnSubmit(instance);
                Db.Faqs.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateFaq(Faq instance)
        {
            var cache = Db.Faqs.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Header = instance.Header;
                cache.Text = instance.Text;
                cache.OrderBy = instance.OrderBy;
                Db.Faqs.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFaq(int idFaq)
        {
            Faq instance = Db.Faqs.FirstOrDefault(p => p.ID == idFaq);
            if (instance != null)
            {
                foreach (var forFaq in Faqs.Where(p => p.OrderBy > instance.OrderBy))
                {
                    forFaq.OrderBy--;
                }
                Db.Faqs.DeleteOnSubmit(instance);
                Db.Faqs.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool MoveFaq(int id, int placeBefore)
        {
            var Faq = Db.Faqs.FirstOrDefault(p => p.ID == id);
            if (Faq != null)
            {
                if (Faq.OrderBy > placeBefore)
                {
                    foreach (var forFaq in Faqs.Where(w => w.OrderBy >= placeBefore && w.OrderBy < Faq.OrderBy))
                    {
                        forFaq.OrderBy++;
                    }
                }

                if (Faq.OrderBy < placeBefore)
                {
                    foreach (var forCatalog in Faqs.Where(w => w.OrderBy > Faq.OrderBy && w.OrderBy <= placeBefore))
                    {
                        forCatalog.OrderBy--;
                    }
                }
                Faq.OrderBy = placeBefore;
                Db.Faqs.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}