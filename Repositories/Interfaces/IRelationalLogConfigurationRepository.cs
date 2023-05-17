using IntegrationLogger.Models.Configuration;

namespace IntegrationLogger.Repositories.Interfaces;
public interface IRelationalLogConfigurationRepository
{
    Task<LogConfiguration?> GetLogConfigurationByIdAsync(Guid id);
    Task<LogConfiguration?> GetLogConfigurationBySourceAsync(string source);
    Task<List<LogConfiguration>> GetLogConfigurationsAsync();
    Task UpdateLogConfigurationAsync(LogConfiguration logConfiguration);
    Task CreateLogConfigurationAsync(LogConfiguration logConfiguration);
    Task DeleteLogConfigurationAsync(LogConfiguration logConfiguration);
}