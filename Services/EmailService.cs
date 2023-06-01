using FastEndpoints;
using System.Net.Mail;
using System.Net;
using Authentication.Properties;
using Microsoft.Extensions.Options;

namespace Authentication.Services
{
    public class EmailService
    {
        private readonly IOptions<SmtpConfig> smtpConfig;
        private readonly SmtpClient smtpClient;
        public EmailService(IOptions<SmtpConfig> smtpConfig)
        {
            this.smtpConfig = smtpConfig;
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(smtpConfig.Value.EmailAddress, smtpConfig.Value.EmailPassword),
                EnableSsl = true
            };
        }
        public bool SendEmail(string toEmail, string emailSubject, string emailBodyHtml)
        {
            if (smtpConfig.Value.EmailAddress == null)
            {
                return false;
            }
            string? testEmail = smtpConfig.Value.TestEmail;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpConfig.Value.EmailAddress),
                Subject = emailSubject,
                Body = emailBodyHtml,
                IsBodyHtml = true,
            };
            if (testEmail != null && testEmail.Length > 0)
            {
                mailMessage.To.Add(testEmail);
            }
            else
            {
                mailMessage.To.Add(toEmail);
            }
            smtpClient.Send(mailMessage);
            return true;
        }
    }
}
