using System.Globalization;
using System.Linq;
using System.Configuration;

namespace platformAthletic.Global.Config
{
    public class Config : IConfig
    {
        protected delegate string GetAppSettingsDelegate(string appSetting);

        protected delegate string GetConnectionStringsDelegate(string connectionString);

        protected delegate ConfigurationSection GetSectionDelegate(string sectionName);

        protected event GetAppSettingsDelegate GetAppSettings;

        protected event GetConnectionStringsDelegate GetConnectionStrings;

        protected event GetSectionDelegate GetSection;

        private Configuration _configuration;

        public Config()
        {
            GetAppSettings += new GetAppSettingsDelegate((appSetting) =>
            {
                return ConfigurationManager.AppSettings[appSetting];
            });

            GetSection += new GetSectionDelegate((sectionName) =>
            {
                return (ConfigurationSection)ConfigurationManager.GetSection(sectionName);
            });

            GetConnectionStrings += new GetConnectionStringsDelegate((connectionString) =>
            {
                return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            });
        }

        public Config(Configuration configuration)
        {
            _configuration = configuration;

            GetAppSettings += new GetAppSettingsDelegate((appSetting) =>
            {
                return _configuration.AppSettings.Settings[appSetting].Value;
            });

            GetSection += new GetSectionDelegate((sectionName) =>
            {
                return _configuration.GetSection(sectionName);
            });

            GetConnectionStrings += new GetConnectionStringsDelegate((connectionString) =>
            {
                return _configuration.ConnectionStrings.ConnectionStrings[connectionString].ConnectionString;
            });
        }

        private string _appSettings(string appSetting)
        {

            return GetAppSettings(appSetting);
        }

        private ConfigurationSection _section(string sectionName)
        {
            return GetSection(sectionName);
        }

        private string _connectionStrings(string connectionString)
        {
            return GetConnectionStrings(connectionString);
        }


        #region IConfig Members

        public string ConnectionStrings(string connectionString)
        {
            return _connectionStrings(connectionString);
        }

        public string CultureCode
        {
            get
            {
                var culture = _appSettings("Culture");
                if (culture != null)
                {
                    return culture;
                }
                return "en";
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return new CultureInfo(CultureCode);
            }
        }
        public bool DebugMode
        {
            get
            {
                return bool.Parse(_appSettings("DebugMode"));
            }
        }

        public string AdminEmail
        {
            get
            {
                return _appSettings("AdminEmail");
            }
        }

        public bool EnableMail
        {
            get
            {
                return bool.Parse(_appSettings("EnableMail")); 
            }
        }

        public bool EnableHttps
        {
            get { 
                return bool.Parse(_appSettings("EnableHttps")); 
            }
        }

        public IQueryable<MimeType> MimeTypes
        {
            get
            {
                MimeTypesConfigSection configInfo = (MimeTypesConfigSection)_section("mimeConfig");
                return configInfo.mimeTypes.OfType<MimeType>().AsQueryable<MimeType>();
            }
        }

        public MailSettings MailSettings
        {
            get
            {
                return (MailSettings)_section("mailConfig");
            }
        }


        public IQueryable<MailTemplate> MailTemlates
        {
            get
            {
                MailTemplateConfig configInfo = (MailTemplateConfig)_section("mailTemplatesConfig");
                return configInfo.mailTemplates.OfType<MailTemplate>().AsQueryable<MailTemplate>();
            }
        }

        public IQueryable<IconSize> IconSizes
        {
            get
            {
                IconSizesConfigSection configInfo = (IconSizesConfigSection)_section("iconConfig");
                if (configInfo != null)
                {
                    return configInfo.iconSizes.OfType<IconSize>().AsQueryable<IconSize>();
                }
                    return null;
            }
        }

        public AuthorizeSettings Authorize
        {
            get
            {
                return (AuthorizeSettings)_section("authorizeConfig");
            }
        }

        #endregion


      
    }
}