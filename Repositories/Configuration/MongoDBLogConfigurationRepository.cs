using IntegrationLogger.Models.Configuration;
using IntegrationLogger.Repositories.Interfaces;

namespace IntegrationLogger.Repositories.Configuration;
public class MongoDBLogConfigurationRepository : IRelationalLogConfigurationRepository
{
    public Task CreateLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        throw new NotImplementedException();
    }

    public Task<LogConfiguration?> GetLogConfigurationByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<LogConfiguration?> GetLogConfigurationBySourceAsync(string source)
    {
        throw new NotImplementedException();
    }

    public Task<List<LogConfiguration>> GetLogConfigurationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        throw new NotImplementedException();
    }
}