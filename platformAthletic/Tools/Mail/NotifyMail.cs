using System;
using System.Linq;
using platformAthletic.Global.Config;
using System.Net.Mail;

namespace platformAthletic.Tools.Mail
{
    public static class NotifyMail
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SendNotify<T>(IConfig config, string templateName, string email, Func<T, string, string> subject, Func<T, string, string> body, T obj, MailAddress mailAddress = null)
        {
            var template = config.MailTemlates.FirstOrDefault(p => string.Compare(p.Name, templateName, true) == 0);
            if (template == null)
            {
                logger.Error("Can't find template (" + templateName + ")");
            }
            else
            {
                MailSender.SendMail(email,
                    subject.Invoke(obj, template.Subject),
                    body.Invoke(obj, template.Template), mailAddress);
            }
        }


    }
}