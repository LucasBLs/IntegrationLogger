using IntegrationLogger.Enums;
using IntegrationLogger.Models;

namespace IntegrationLogger.Interfaces;

public interface IIntegrationLogService
{
    IntegrationLog Log(string integrationName, string message, string externalSystem, string sourceSystem);
    IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem);
    IntegrationDetail AddDetail(IntegrationLog log, IntegrationStatus status, string detailIdentifier, string? message, string? errorMessage);
    IntegrationItem AddItem(IntegrationDetail detail, ItemType itemType, string itemIdentifier, IntegrationStatus itemStatus, string? message, object? errorMessage);
    Task<List<IntegrationLog>> SearchLogs(DateTimeOffset startDate, DateTimeOffset endDate, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null);
    Task<List<IntegrationDetail>> GetDetailsByLogId(Guid logId);
    Task<List<IntegrationItem>> GetItemsByDetailId(Guid detailId);
}
