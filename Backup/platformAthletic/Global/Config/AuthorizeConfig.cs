using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace platformAthletic.Global.Config
{
    public class AuthorizeSettings : ConfigurationSection
    {
        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["Url"] as string;
            }
            set
            {
                this["Url"] = value;
            }
        }



        // Create a "LoginID" attribute.
        [ConfigurationProperty("LoginID", IsRequired = true)]
        public string LoginID
        {
            get
            {
                return this["LoginID"] as string;
            }
            set
            {
                this["LoginID"] = value;
            }
        }

        // Create a "TransactionKey" attribute.
        [ConfigurationProperty("TransactionKey", IsRequired = true)]
        public string TransactionKey
        {
            get
            {
                return this["TransactionKey"] as string;
            }
            set
            {
                this["TransactionKey"] = value;
            }
        }
    }
}