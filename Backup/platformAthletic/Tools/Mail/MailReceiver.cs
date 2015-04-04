using OpenPop.Mime;
using OpenPop.Pop3;
using platformAthletic.Global.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Tools.Mail
{
    public class MailReceiver
    {
        public static IConfig config;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static Dictionary<int, Message> GetMails()
        {
            try
            {
                // The client disconnects from the server when being disposed
                using (var client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(config.MailSettings.Pop3Server,
                        config.MailSettings.Pop3Port,
                       config.MailSettings.EnableSsl);

                    // Authenticate ourselves towards the server
                    client.Authenticate(
                        config.MailSettings.Pop3UserName,
                        config.MailSettings.Pop3Password);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();

                    // We want to download all messages
                    Dictionary<int, Message> allMessages = new Dictionary<int, Message>(messageCount);

                    // Messages are numbered in the interval: [1, messageCount]
                    // Ergo: message numbers are 1-based.
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        allMessages.Add(i, client.GetMessage(i));
                    }

                    // Now return the fetched messages
                    return allMessages;
                }
            }
            catch
            {
                return new Dictionary<int, Message>();
            }
        }

        public static void RemoveMessage(int messageNum)
        {
            try
            {
                // The client disconnects from the server when being disposed
                using (var client = new Pop3Client())
                {
                    client.DeleteMessage(messageNum);
                }
            } catch  {

            }
        }
    }
}