using IntegrationLogger.Enums;

namespace IntegrationLogger.Models.Configuration;
public class LogConfiguration
{
    public Guid Id { get; set; }
    public string? LogSource { get; set; }
    public LogLevel LogLevel { get; set; }
    public bool LogStepByStep { get; set; }
    public int LogRetentionPeriod { get; set; }
    public bool AutoArchive { get; set; }
    public string? ArchivePath { get; set; }
    public bool EmailNotification { get; set; }
    public EmailConfiguration EmailConfiguration { get; set; } = new();
}