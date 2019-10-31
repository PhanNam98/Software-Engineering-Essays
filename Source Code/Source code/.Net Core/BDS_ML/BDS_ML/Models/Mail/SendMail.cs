using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace BDS_ML.Models.Mail
{
    public class SendMail
    {
       private SendMail() { }
        public static void sendMail(string Message,string email,string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BDS_ML Service", "thienquoc98@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = Message
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("thienquoc98@gmail.com", "wbqkfozujxafrjoa");

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
