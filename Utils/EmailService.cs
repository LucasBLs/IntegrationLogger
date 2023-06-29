using IntegrationLogger.Models.Configuration;
using System.Net;
using System.Net.Mail;

namespace IntegrationLogger.Utils;
public class EmailService
{
    public static async Task SendEmail(EmailConfiguration emailConfiguration)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailConfiguration.SenderEmail, emailConfiguration.SenderName)
        };
        mailMessage.To.Add(emailConfiguration.RecipientEmail);
        var ccEmails = emailConfiguration.CcEmails.Split(',').ToList();
        ccEmails.ForEach(ccEmail => mailMessage.CC.Add(ccEmail));
        mailMessage.Body = emailConfiguration.EmailBody;
        mailMessage.Subject = emailConfiguration.EmailSubject;

        using var smtpClient = new SmtpClient(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort);
        smtpClient.Credentials = new NetworkCredential(emailConfiguration.SenderEmail, emailConfiguration.EmailPassword);
        smtpClient.EnableSsl = true;
        await smtpClient.SendMailAsync(mailMessage);
    }
}