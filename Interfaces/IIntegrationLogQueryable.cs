using IntegrationLogger.Models;

namespace IntegrationLogger.Interfaces;
public interface IIntegrationLogQueryable
{
    IQueryable<IntegrationLog> GetIntegrationLogs();
    IQueryable<IntegrationDetail> GetIntegrationDetails();
    IQueryable<IntegrationItem> GetIntegrationItems();
}