using IntegrationLogger.Enums;

namespace IntegrationLogger.Configuration;
public class IntegrationLoggerConfiguration
{
    public string ConnectionString { get; set; } = default!;
    public DatabaseProvider Provider { get; set; }
    public string? MongoDatabaseName { get; set; }
    public string? Roles { get; set; }
    public string? MigrationsDirectory { get; set; }
}
