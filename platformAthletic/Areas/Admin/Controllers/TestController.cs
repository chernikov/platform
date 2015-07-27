using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class TestController : AdminController
    {

        public ActionResult Index(string email = "chernikov@gmail.com")
        {
            var mailAddress = new MailAddress(Config.MailSettings.SmtpReply, Config.MailSettings.SmtpUser);

            MailMessage message = new MailMessage(
                       mailAddress,
                       new MailAddress(email))
            {
                Subject = "Test subject",
                BodyEncoding = Encoding.UTF8,
                Body = "Test Body",
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8
            };

            SmtpClient client = new SmtpClient
            {
                Host = Config.MailSettings.SmtpServer,
                Port = Config.MailSettings.SmtpPort,
                UseDefaultCredentials = false,
                EnableSsl = Config.MailSettings.EnableSsl,
                Credentials =
                    new NetworkCredential(Config.MailSettings.SmtpUserName,
                                          Config.MailSettings.SmtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(message);


            return Content("OK");
        }

    }
}
