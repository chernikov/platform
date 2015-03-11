using platformAthletic.Global.Config;
using platformAthletic.Model;
using platformAthletic.Tools.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace platformAthletic
{
    public class FailMessage
    {
        public static string RegexEmail = @"\b([A-Z0-9._%-]+)@([A-Z0-9.-]+\.[A-Z]{2,6})\b";


        public static void Check(IRepository repository)
        {
           /* var messages = MailReceiver.GetMails();

            foreach (var message in messages)
            {
                var mailMessage = message.Value.ToMailMessage();

                if (string.Compare(mailMessage.Subject, "Delivery Status Notification (Failure)", true) == 0)
                {
                    var body = mailMessage.Body;

                    var emailMatches = Regex.Matches(body, RegexEmail, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    if (emailMatches.Count > 0)
                    {
                        foreach (Match match in emailMatches)
                        {
                            var email = match.Value;
                            var exist = repository.FailedMails.Any(p => string.Compare(p.FailEmail, email, true) == 0);

                            if (!exist)
                            {
                                var failMail = new FailedMail
                                {
                                    AddedDate = DateTime.Now,
                                    FailEmail = email, 
                                    Subject = mailMessage.Subject, 
                                    Body = mailMessage.Body,
                                    IsProcessed = false
                                };
                                repository.CreateFailedMail(failMail);
                                MailReceiver.RemoveMessage(message.Key);
                                break;
                            }
                        }
                    }
                }
            }*/
        }

        public static void Process(IRepository repository, IConfig config)
        {
            while (true)
            {
                var email = repository.PopFailedMail();
                if (email != null)
                {
                    var user = repository.Users.FirstOrDefault(p => string.Compare(p.Email, email.FailEmail, true) == 0); 
                    if (user != null) 
                    {
                        var team = user.Team;
                        if (team != null) 
                        {
                            var coach = team.User;

                            if (coach != null) {
                                NotifyMail.SendNotify<User>(config, "MailFailed", coach.Email,
                                    (u, format) => email.Subject,
                                    (u, format) => email.Body,
                                    user);
                            }
                        }
                        
                    }
                  
                }
                else
                {
                    break;
                }
            }
        }
    }
}