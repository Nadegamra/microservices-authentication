using FastEndpoints;
using System.Net.Mail;
using System.Net;

namespace Authentication.Services
{
    public class EmailService
    {
        private readonly IConfiguration config;
        private readonly SmtpClient smtpClient;
        public EmailService(IConfiguration config)
        {
            this.config = config;
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(config["EmailAddress"], config["EmailPassword"]),
                EnableSsl = true
            };
        }
        public bool SendEmail(string toEmail, string emailSubject, string emailBodyHtml)
        {
            if (config["EmailAddress"] == null)
            {
                return false;
            }
            string? testEmail = config["TestEmail"];
            var mailMessage = new MailMessage
            {
                From = new MailAddress(config["EmailAddress"]!),
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
