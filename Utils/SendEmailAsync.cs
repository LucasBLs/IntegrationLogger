using IntegrationLogger.Models;
using System.Net.Mail;
using System.Net;

namespace IntegrationLogger.Utils;
public class SendEmail
{
    public async Task SendEmailAsync(LogConfiguration config, string subject, string body)
    {
        using var smtpClient = new SmtpClient(config.EmailHost, config.EmailPort)
        {
            Credentials = new NetworkCredential(config.EmailUsername, config.EmailPassword),
            EnableSsl = config.EmailUseSSL
        };

        if(!string.IsNullOrEmpty(config.EmailUsername) && !string.IsNullOrEmpty(config.EmailRecipients))
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.EmailUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var recipient in config.EmailRecipients.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(recipient.Trim());
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
       
    }
}