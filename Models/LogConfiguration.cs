namespace IntegrationLogger.Models;
public class LogConfiguration
{
    public Guid Id { get; set; }
    public string? LogSource { get; set; }
    public int LogRetentionPeriod { get; set; }
    public bool AutoArchive { get; set; }
    public string? ArchivePath { get; set; }
    public bool EmailNotification { get; set; }
    public string? EmailRecipients { get; set; }
    public string? EmailHost { get; set; }
    public int EmailPort { get; set; }
    public string? EmailUsername { get; set; }
    public string? EmailPassword { get; set; }
    public bool EmailUseSSL { get; set; }
}