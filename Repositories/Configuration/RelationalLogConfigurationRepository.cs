using IntegrationLogger.Data;
using IntegrationLogger.Models.Configuration;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Repositories.Configuration;
public class RelationalLogConfigurationRepository : IRelationalLogConfigurationRepository
{
    private readonly IntegrationLogContextBase _context;
    public RelationalLogConfigurationRepository(IntegrationLogContextBase context)
        => _context = context;

    public async Task<LogConfiguration?> GetLogConfigurationByIdAsync(Guid id)
         => await _context.LogConfigurations.FirstOrDefaultAsync(x => x.Id == id);
    public async Task<LogConfiguration?> GetLogConfigurationBySourceAsync(string source)
            => await _context.LogConfigurations.FirstOrDefaultAsync(x => x.IntegrationLog!.IntegrationName == source);
    public async Task<List<LogConfiguration>> GetLogConfigurationsAsync()
         => await _context.LogConfigurations.AsNoTracking().ToListAsync();
    public async Task UpdateLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        _context.LogConfigurations.Update(logConfiguration);
        await _context.SaveChangesAsync();
    }
    public async Task CreateLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        await _context.LogConfigurations.AddAsync(logConfiguration);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteLogConfigurationAsync(LogConfiguration logConfiguration)
    {
        _context.LogConfigurations.Remove(logConfiguration);
        await _context.SaveChangesAsync();
    }
}