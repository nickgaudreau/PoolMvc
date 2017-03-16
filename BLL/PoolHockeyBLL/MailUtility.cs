using System;
using System.Net;
using System.Net.Mail;
using PoolHockeyBLL.Log;

namespace PoolHockeyBLL
{
    public class MailUtility
    {
        private const string Smtp = "mail.poolhockey.info";
        private const int SslPort = 465;
        private const int Port = 25;
        private const string UserEmail = "postmaster@poolhockey.info";
        private const string Pass = "Carlyto3#";

        public static bool SendMail(string body, string subject)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(Smtp, Port); // host, port
                smtpClient.Credentials = new NetworkCredential(UserEmail, Pass);
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();
                //Setting From , To and CC
                mail.From = new MailAddress("nickgaudreau82@gmail.com", subject);
                mail.To.Add(new MailAddress(UserEmail));
                mail.Body = body;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                LogError.Write(e, $"Fail to send the email to: {UserEmail} at smtp address: {Smtp}, with ssl port: {SslPort}");
                return false;
            }

            
        }

    }
}
