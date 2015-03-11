using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class FeatureCatalog
    {
        public FeatureCatalog Parent { get; set; }

        public IEnumerable<FeatureText> SubTexts
        {
            get
            {
                return FeatureTexts.OrderBy(p => p.OrderBy).AsEnumerable();
            }
        }
	}
}