using System.Linq;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using platformAthletic.Controllers;
using platformAthletic.Model;
using System;
using platformAthletic.Helpers;
using System.Collections.Generic;
using platformAthletic.Models.Info;
using System.IO;
using platformAthletic.Tools.Mail;

namespace platformAthletic.Areas.Default.Controllers
{
    public abstract class DefaultController : BaseController
    {
        protected static List<string> PayedControllers = new List<string>() {
            "Player", "Video", "Faq", "GettingStarted", "LeaderBoard", "Report", "Table", "Team"
        };

        protected bool ProcessPayment(BillingInfo billingInfo, double amount, string ReferralCode, PromoAction.TargetEnum target)
        {
           
            var user = Repository.Users.FirstOrDefault(p => p.ID == billingInfo.UserID);
            if (user != null)
            {
                var paymentDate = (user.PaidTill.HasValue && user.PaidTill.Value > DateTime.Now.Current()) ?
                       user.PaidTill.Value.AddYears(1)
                       : DateTime.Now.Current().AddYears(1);

                /* create payment */
                var paymentDetail = new PaymentDetail
                {
                    UserID = billingInfo.UserID,
                    ReferralCode = ReferralCode,
                    Amount = amount,
                    Description = string.Format("Payment (Credit cart) User ID {0} till : {1}", user.Email, paymentDate),
                };

                Repository.CreatePaymentDetail(paymentDetail);
                /* process credit cart */
                if (billingInfo.State == null)
                {
                    billingInfo.State = Repository.States.ToList().FirstOrDefault(p => p.ID == billingInfo.StateID);
                }
                if (TakeMoney(billingInfo, amount, paymentDetail))
                {
                    /* use promocode */
                    if (!string.IsNullOrWhiteSpace(ReferralCode))
                    {
                        Repository.UsePromoCode(ReferralCode);
                    }

                    user.PaidTill = paymentDate;
                    Repository.UpdatePaidTillUser(user);
                    return true;
                }
            }
            return false;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.RouteData.Values["controller"] != null)
            {
                if (CurrentUser != null && !CurrentUser.InRoles("payed") &&
                    PayedControllers.Contains(requestContext.RouteData.Values["controller"] as string))
                {
                    requestContext.HttpContext.Response.Redirect("~/billing");
                }
            }
            base.Initialize(requestContext);
        }

        protected void ProcessPayment(Invoice invoice, double amount, string ReferralCode, PromoAction.TargetEnum target)
        {
            /* use promocode */
            if (!string.IsNullOrWhiteSpace(ReferralCode))
            {
                Repository.UsePromoCode(ReferralCode);
            }
            var user = Repository.Users.FirstOrDefault(p => p.ID == invoice.UserID);
            if (user != null)
            {
                var paymentDate = (user.PaidTill.HasValue && user.PaidTill.Value > DateTime.Now.Current()) ?
                        user.PaidTill.Value.AddYears(1)
                        : DateTime.Now.Current().AddYears(1);
                /* create payment */
                var paymentDetail = new PaymentDetail
                {
                    UserID = user.ID,
                    ReferralCode = ReferralCode,
                    Amount = amount,
                    Description = string.Format("Payment (Invoice) User ID {0} till : {1}", user.Email, paymentDate)
                };
                Repository.CreatePaymentDetail(paymentDetail);
                user.PaidTill = paymentDate;
                Repository.UpdatePaidTillUser(user);
            }
        }


        protected void SendWelcomeCoachMail(string email, string subject, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                UserName = username, 
                Password = password
            };
            var mail = mailController.WelcomeCoach(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            MailSender.SendMail(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
        }

        protected bool SendWelcomePlayerMail(string email, string subject, string coach, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                CoachName = coach,
                UserName = username,
                Password = password
            };
            var mail = mailController.WelcomePlayer(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            bool result = MailSender.SendMailForPlayer(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
            return result;
        }

        protected void SendWelcomeAssistantMail(string email, string subject, string coach, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                CoachName = coach,
                UserName = username,
                Password = password
            };
            var mail = mailController.WelcomeAssistant(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            MailSender.SendMail(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
        }

        protected void SendWelcomeIndividualMail(string email, string subject, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                UserName = username,
                Password = password
            };
            var mail = mailController.WelcomeIndividual(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            MailSender.SendMail(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
        }

        protected void SendForgotPasswordMail(string email, string subject, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                UserName = username,
                Password = password
            };
            var mail = mailController.ForgotPassword(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            MailSender.SendMail(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
        }

        protected void SendResendMail(string email, string subject, string username, string password)
        {
            var mailController = new MailController();
            var mailInfo = new MailInfo()
            {
                Subject = subject,
                Email = email,
                UserName = username,
                Password = password
            };
            var mail = mailController.Resend(mailInfo, HostName);
            using (var reader = new StreamReader(mail.Mail.AlternateViews[0].ContentStream))
            {
                mailInfo.Body = reader.ReadToEnd();
            }
            MailSender.SendMail(mailInfo.Email, mailInfo.Subject, mailInfo.Body);
        }
    }
}
