using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class BillingInfo
    {

        public string TextExpDate
        {
            get
            {
                return string.Format("{0}{1}", ExparationDate.Month.ToString("D2"), ExparationDate.ToString("yy"));
            }
        }

        public string FirstName
        {
            get
            {
                var strings = NameOnCard.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                if (strings.Length > 0) {
                    return strings[0];
                }
                return string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                var strings = NameOnCard.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (strings.Length > 1)
                {
                    return strings[1];
                }
                return string.Empty;
            }
        }
	}
}