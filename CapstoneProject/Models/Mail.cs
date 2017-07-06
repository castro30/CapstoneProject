using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;

namespace CapstoneProject.Models
{
    public static class Mail
    {
        public static void SendEmail(string toAddress, string subject, string body)
        {
            using (SmtpClient client = new SmtpClient("mail.smtp2go.com", 443))
            {
                client.EnableSsl = true;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("castro30@yahoo.com", "Wjx0agHlt5UQ");
                //client.UseDefaultCredentials = false;

                using (MailMessage message = new MailMessage("castro30@yahoo.com", toAddress, subject, body))
                {                    //message.BodyEncoding = UTF8Encoding.UTF8;

                    client.Send(message);
                }
            }
                
        }
    }
}