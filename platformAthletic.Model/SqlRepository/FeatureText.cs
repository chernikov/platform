using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<FeatureText> FeatureTexts
        {
            get
            {
                return Db.FeatureTexts;
            }
        }

        public bool CreateFeatureText(FeatureText instance)
        {
            if (instance.ID == 0)
            {

                var lastOrderBy = FeatureTexts.Where(p => p.FeatureCatalogID == instance.FeatureCatalogID).OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                instance.OrderBy = lastOrderBy + 1;

                Db.FeatureTexts.InsertOnSubmit(instance);
                Db.FeatureTexts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateFeatureText(FeatureText instance)
        {
            var cache = Db.FeatureTexts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.FeatureCatalogID = instance.FeatureCatalogID;
				cache.OrderBy = instance.OrderBy;
				cache.Header = instance.Header;
				cache.Text = instance.Text;
                Db.FeatureTexts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveFeatureText(int idFeatureText)
        {
            FeatureText instance = Db.FeatureTexts.FirstOrDefault(p => p.ID == idFeatureText);
            if (instance != null)
            {
                var parentFeatureCatalogs = FeatureCatalogs.FirstOrDefault(p => p.ID == instance.FeatureCatalogID);
                if (parentFeatureCatalogs != null)
                {
                    foreach (var forFeature in FeatureTexts.Where(p => p.FeatureCatalogID == parentFeatureCatalogs.ID && p.OrderBy > instance.OrderBy))
                    {
                        forFeature.OrderBy--;
                    }
                }
                Db.FeatureTexts.DeleteOnSubmit(instance);
                Db.FeatureTexts.Context.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool ChangeParentFeatureText(int id, int idParent)
        {
            var featureText = Db.FeatureTexts.FirstOrDefault(p => p.ID == id);

            if (featureText == null)
            {
                //нету такого - всё пропало
                return false;
            }
            var newParentFeatureCatalog = Db.FeatureCatalogs.FirstOrDefault(p => p.ID == idParent);
            if (newParentFeatureCatalog == null)
            {
                //нету такого - всё пропало
                return false;
            }
            else
            {
                if (featureText.FeatureCatalogID == idParent)
                {
                    //никуда не перемещаем 
                    return true;
                }
                else
                {
                    //пересортировка в бывшем списке
                    ReOrderFeatureTextBeforeMove(featureText);
                    //добавить последним 
                    int lastOrderBy = FeatureTexts.Where(p => p.FeatureCatalogID == featureText.FeatureCatalogID).OrderByDescending(p => p.OrderBy).Select(p => p.OrderBy).FirstOrDefault();
                    featureText.OrderBy = lastOrderBy + 1;
                    featureText.FeatureCatalog = newParentFeatureCatalog;
                    //уииии
                    Db.FeatureTexts.Context.SubmitChanges();
                    return true;
                }
            }
        }

        private void ReOrderFeatureTextBeforeMove(FeatureText feature)
        {
            var parent = feature.FeatureCatalog;
            if (parent != null)
            {
                foreach (var features in parent.FeatureTexts.Where(w => w.OrderBy > feature.OrderBy))
                {
                    features.OrderBy--;
                }
            }
        }

        public bool MoveFeatureText(int id, int placeBefore)
        {
            var FeatureText = Db.FeatureTexts.FirstOrDefault(p => p.ID == id);
            if (FeatureText != null)
            {
                if (FeatureText.OrderBy > placeBefore)
                {
                    foreach (var forFeatureText in FeatureTexts.Where(w => 
                        w.OrderBy >= placeBefore &&
                        w.OrderBy < FeatureText.OrderBy &&
                        w.FeatureCatalogID == FeatureText.FeatureCatalogID))
                    {
                        forFeatureText.OrderBy++;
                    }
                }

                if (FeatureText.OrderBy < placeBefore)
                {
                    foreach (var forFeatureText in FeatureTexts.Where(w => 
                        w.OrderBy > FeatureText.OrderBy && 
                        w.OrderBy <= placeBefore &&
                        w.FeatureCatalogID == FeatureText.FeatureCatalogID))
                    {
                        forFeatureText.OrderBy--;
                    }
                }
                FeatureText.OrderBy = placeBefore;
                Db.FeatureTexts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}