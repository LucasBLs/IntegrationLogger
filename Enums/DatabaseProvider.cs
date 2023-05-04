namespace IntegrationLogger.Enums;

public enum DatabaseProvider
{
    SqlServer,
    PostgreSQL,
    Oracle,
    MongoDB
}

public class ProviderDb
{
    public static DatabaseProvider LoggerDatabaseProvider { get; set; }
}