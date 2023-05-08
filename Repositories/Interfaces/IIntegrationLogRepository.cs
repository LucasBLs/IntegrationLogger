using IntegrationLogger.Enums;
using IntegrationLogger.Models.Integration;

namespace IntegrationLogger.Repositories.Interfaces;

public interface IIntegrationLogRepository
{
    IntegrationLog Log(string integrationName, string message, string externalSystem, string sourceSystem);
    IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem);
    IntegrationDetail AddDetail(IntegrationLog log, IntegrationStatus status, string detailIdentifier, string? message);
    IntegrationItem AddItem(IntegrationDetail detail, ItemType itemType, string itemIdentifier, IntegrationStatus itemStatus, string? message, object? content);
    Task<(List<IntegrationLog> logs, int totalCount)> GetLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null, bool groupByIntegrationName = false, int pageIndex = 1, int pageSize = 0);
    Task<(List<IntegrationDetail> details, int totalCount)> GetDetails(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? detailIdentifier = null, bool filterWithIdentifier = true, string? integrationName = null, int pageIndex = 1, int pageSize = 0);
    Task<List<IntegrationDetail>> GetDetailsByLogId(Guid logId);
    Task<List<IntegrationItem>> GetItemsByDetailId(Guid detailId);
}