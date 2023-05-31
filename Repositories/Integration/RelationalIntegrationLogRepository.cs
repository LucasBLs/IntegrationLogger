using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using IntegrationLogger.Extensions;
using IntegrationLogger.Models.Integration;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Repositories.Integration;
public class RelationalIntegrationLogRepository : IIntegrationLogRepository
{
    private readonly IntegrationLogContextBase _context;
    public RelationalIntegrationLogRepository(IntegrationLogContextBase context)
    {
        _context = context;
    }

    #region AddLogs
    public IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem)
    {
        LogLevel configuredLogLevel = _context.LogConfigurations.FirstOrDefault()?.LogLevel ?? LogLevel.Info;
        if (configuredLogLevel == LogLevel.None)
            return new();

        var project = _context.IntegrationLogs.FirstOrDefault(x => x.IntegrationName == integrationName);
        if (project is not null)
            return project;

        IntegrationLog log = new()
        {
            IntegrationName = integrationName,
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
            Message = message,
            ExternalSystem = externalSystem,
            SourceSystem = sourceSystem,
        };

        _context.IntegrationLogs.Add(log);
        _context.SaveChanges();
        return log;
    }
    public IntegrationDetail AddDetail(IntegrationLog log, IntegrationStatus status, string detailIdentifier, string? message)
    {
        LogLevel configuredLogLevel = _context.LogConfigurations.FirstOrDefault()?.LogLevel ?? LogLevel.Info;
        if (configuredLogLevel == LogLevel.None)
            return new();

        IntegrationDetail detail = new()
        {
            IntegrationLogId = log.Id,
            Status = status,
            DetailIdentifier = detailIdentifier,
            Message = message,
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
            IntegrationLogName = log.IntegrationName,
            Items = new List<IntegrationItem>(),
        };

        log?.Details?.Add(detail);
        _context.IntegrationDetails.Add(detail);
        _context.SaveChanges();
        return detail;
    }
    public IntegrationItem AddItem(IntegrationDetail detail, ItemType itemType, string itemIdentifier, IntegrationStatus itemStatus, string? message, object? content)
    {
        LogLevel configuredLogLevel = _context.LogConfigurations.FirstOrDefault()?.LogLevel ?? LogLevel.Info;
        if (configuredLogLevel == LogLevel.None)
            return new();

        IntegrationItem item = new()
        {
            IntegrationDetailId = detail.Id,
            ItemType = itemType,
            ItemIdentifier = itemIdentifier,
            ItemStatus = itemStatus,
            Message = message,
            Content = content.SerializeIndentedObject(),
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
        };

        if (item.ItemStatus == IntegrationStatus.Failed)
        {
            detail.Status = IntegrationStatus.Failed;
            _context.IntegrationDetails?.Update(detail);
            _context.SaveChanges();
        }

        detail?.Items?.Add(item);
        _context.IntegrationItems.Add(item);
        _context.SaveChanges();
        return item;
    }
    #endregion

    #region GetLogs
    public async Task<(List<IntegrationLog> logs, int totalCount)> GetLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null, bool groupByIntegrationName = false, int pageIndex = 1, int pageSize = 0)
    {
        var query = _context.IntegrationLogs.AsNoTracking().AsQueryable();
        
        if(startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);
        
        if (!string.IsNullOrEmpty(integrationName))
            query = query.Where(l => l.IntegrationName != null && l.IntegrationName.ToLower().Contains(integrationName.ToLower()));

        if (!string.IsNullOrEmpty(externalSystem))
            query = query.Where(l => l.ExternalSystem != null && l.ExternalSystem.Contains(externalSystem));
        
        if (!string.IsNullOrEmpty(sourceSystem))
            query = query.Where(l => l.SourceSystem != null && l.SourceSystem.Contains(sourceSystem));

        int totalCount = await query.CountAsync();

        if (pageSize > 0)
            query = query.OrderByDescending(x => x.Timestamp).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var logs = await query.ToListAsync();

        return (logs, totalCount);
    }
    public async Task<(List<IntegrationDetail> details, int totalCount)> GetDetails(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? detailIdentifier = null, bool filterWithIdentifier = true, string? integrationName = null, Guid? integrationLogId = null, int pageIndex = 1, int pageSize = 0)
    {
        var query = _context.IntegrationDetails.AsNoTracking().AsQueryable();

        if (integrationLogId != null)
            query = query.Where(l => l.IntegrationLogId == integrationLogId);

        if (startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(detailIdentifier))
            query = query.Where(l => l.DetailIdentifier!= null && l.DetailIdentifier.ToLower().Contains(detailIdentifier.ToLower()));

        if (filterWithIdentifier is true)
            query = query.Where(x => x.DetailIdentifier != null);

        if (integrationName is not null && integrationName.Any())
            query = query.Where(delivery => delivery.IntegrationLogName != null && integrationName.Contains(delivery.IntegrationLogName));

        int totalCount = await query.CountAsync();

        if (pageSize > 0)
            query = query.OrderByDescending(x => x.Timestamp).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var details = await query.ToListAsync();

        return (details, totalCount);
    }
    public async Task<List<IntegrationItem>> GetItemsByDetailId(Guid detailId)
    {
        return await _context.IntegrationItems.AsNoTracking()
            .Where(i => i.IntegrationDetailId == detailId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
    #endregion
}