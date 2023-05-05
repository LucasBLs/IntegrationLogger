using IntegrationLogger.Configuration;
using IntegrationLogger.Enums;
using IntegrationLogger.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationLogger.Services.ApiGateway;
public static class ApiGatewayLogServiceFactory
{
    public static IApiGatewayLogService CreateApiGatewayLogService(IntegrationLoggerConfiguration config, IServiceProvider serviceProvider)
    {
        if (config.Provider == DatabaseProvider.MongoDB)
        {
            if (config.MongoDatabaseName == null)
                throw new ArgumentNullException(config.MongoDatabaseName, "Set value to MongoDatabaseName.");
            return serviceProvider.GetRequiredService<MongoDBApiGatewayLogService>();
        }
        else
            return serviceProvider.GetRequiredService<RelactionalApiGatewayLogService>();
    }
}