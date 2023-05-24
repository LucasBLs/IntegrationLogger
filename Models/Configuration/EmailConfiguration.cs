namespace IntegrationLogger.Models.Configuration;
public class EmailConfiguration
{
    public Guid Id { get; set; }
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string? EmailPassword { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string CcEmails { get; set; } = string.Empty;
    public string? EmailSubject { get; set; }
    public string? EmailBody { get; set; }
    public LogConfiguration LogConfiguration { get; set; } = new();
    public Guid LogConfigurationId { get; set; }
    
}

