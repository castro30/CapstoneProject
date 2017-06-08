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
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("charwell1234@gmail.com", "password@123");
            client.UseDefaultCredentials = false;

            MailMessage message = new MailMessage("charwell1234@gmail.com", toAddress, subject,body);
            message.BodyEncoding = UTF8Encoding.UTF8;

            client.Send(message);

                
        }
    }
}