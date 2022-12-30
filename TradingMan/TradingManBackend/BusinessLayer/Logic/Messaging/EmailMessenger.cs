using System.Net;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.Util;

namespace TradingManBackend.Logic.Messaging
{
    /// <summary>
    /// Class for sending email messages.
    /// </summary>
    public class EmailMessenger
    {
        public static void SendEmail(string recipient, Message message)
        {
            System.Diagnostics.Debug.WriteLine("Sending Email");

            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Constants.GmailEmail, Constants.GmailPassword),
                EnableSsl = true
            };

            client.Send(Constants.GmailEmail, recipient, message.Subject, message.Body);
            System.Diagnostics.Debug.WriteLine("Mail Sent.");
        }
    }
}
