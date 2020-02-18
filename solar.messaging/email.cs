using solar.generics.DataHelper;
using System;
using System.Net;
using System.Net.Mail;
using solar.messaging.Model;

namespace solar.messaging
{
    public static class Email
    {
        public static EmailSettings settings { get; set; }

        public static bool Send(string email, string subject, string message)
        {
            string toEmail = string.IsNullOrEmpty(email)
                ? settings.ToEmail
                : email;
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(settings.UsernameEmail, "Simbayu System Email")
            };
            mail.To.Add(new MailAddress(toEmail));
            mail.CC.Add(new MailAddress(settings.CcEmail));

            mail.Subject = "System Generated Email - " + subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using (SmtpClient smtp = new SmtpClient(settings.SecondayDomain, settings.SecondaryPort))
            {
                smtp.Credentials = new NetworkCredential(settings.UsernameEmail, settings.UsernamePassword);
                smtp.EnableSsl = true;
                AsyncHelpers.RunSync(()=> smtp.SendMailAsync(mail));
                return true;
            }
        }
    }
}
