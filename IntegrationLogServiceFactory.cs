using IntegrationLogger.Configuration;
using IntegrationLogger.Enums;
using IntegrationLogger.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationLogger;
public static class IntegrationLogServiceFactory
{
    public static IIntegrationLogService CreateIntegrationLogService(IntegrationLoggerConfiguration config, IServiceProvider serviceProvider)
    {
        if (config.Provider == DatabaseProvider.MongoDB)
        {
            if (config.MongoDatabaseName == null)
                throw new ArgumentNullException(config.MongoDatabaseName, "Set value to MongoDatabaseName.");
            return serviceProvider.GetRequiredService<MongoDBIntegrationLogService>();
        }
        else
            return serviceProvider.GetRequiredService<RelationalIntegrationLogService>();
    }
}
