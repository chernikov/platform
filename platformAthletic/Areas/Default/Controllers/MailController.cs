using ActionMailer.Net.Mvc;
using platformAthletic.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult WelcomeCoach(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("WelcomeCoach", mail);
        }

        public EmailResult WelcomePlayer(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("WelcomePlayer", mail);
        }

        public EmailResult WelcomeAssistant(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("WelcomeAssistant", mail);
        }

        public EmailResult WelcomeIndividual(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("WelcomeIndividual", mail);
        }

        public EmailResult ForgotPassword(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("ForgotPassword", mail);
        }

        public EmailResult Resend(MailInfo mail, string host)
        {
            To.Add(mail.Email);
            Subject = mail.Subject;
            MessageEncoding = Encoding.UTF8;
            mail.Host = host;
            return Email("Resend", mail);
        }

        public EmailResult ServerError(Dictionary<string,object> data)
        {                        
            To.Add(data["Email"].ToString());
            Subject = data["Subject"].ToString();
            MessageEncoding = Encoding.UTF8;  
            return Email("ServerError", data);
        }

    }
}
