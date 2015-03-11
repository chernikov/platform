using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace platformAthletic.Global.Config
{
    public interface IConfig
    {
        string ConnectionStrings(string connectionString);

        string CultureCode { get; }

        CultureInfo Culture { get; }

        bool DebugMode { get; }

        string AdminEmail { get; }

        IQueryable<MimeType> MimeTypes { get; }

        MailSettings MailSettings { get; }

        bool EnableMail { get; }

        IQueryable<MailTemplate> MailTemlates { get; }

        IQueryable<IconSize> IconSizes { get; }

        AuthorizeSettings Authorize { get; }

        bool EnableHttps { get; }
    }
}
