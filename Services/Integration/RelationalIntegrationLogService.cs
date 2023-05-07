using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using IntegrationLogger.Interfaces;
using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IntegrationLogger.Services.Integration;
public class RelationalIntegrationLogService : IIntegrationLogService, IIntegrationLogQueryable
{
    private readonly IntegrationLogContextBase _context;
    public RelationalIntegrationLogService(IntegrationLogContextBase context)
    {
        _context = context;
    }

    #region AddLogs
    public IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem)
    {
        return Log(integrationName, message, externalSystem, sourceSystem);
    }
    public IntegrationLog Log(string integrationName, string message, string externalSystem, string sourceSystem)
    {
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
        IntegrationDetail detail = new()
        {
            IntegrationLogId = log.Id,
            Status = status,
            DetailIdentifier = detailIdentifier,
            Message = message,
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
            Items = new List<IntegrationItem>(),
        };

        log?.Details?.Add(detail);
        _context.IntegrationDetails.Add(detail);
        _context.SaveChanges();
        return detail;
    }
    public IntegrationItem AddItem(IntegrationDetail detail, ItemType itemType, string itemIdentifier, IntegrationStatus itemStatus, string? message, object? content)
    {
        IntegrationItem item = new()
        {
            IntegrationDetailId = detail.Id,
            ItemType = itemType,
            ItemIdentifier = itemIdentifier,
            ItemStatus = itemStatus,
            Message = message,
            Content = JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }),
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
    public async Task<List<IntegrationLog>> SearchLogs(DateTimeOffset startDate, DateTimeOffset endDate, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null)
    {
        var query = _context.IntegrationLogs.AsNoTracking().AsQueryable().Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(integrationName))
        {
            query = query.Where(l => l.IntegrationName == integrationName);
        }

        if (!string.IsNullOrEmpty(externalSystem))
        {
            query = query.Where(l => l.ExternalSystem == externalSystem);
        }

        if (!string.IsNullOrEmpty(sourceSystem))
        {
            query = query.Where(l => l.SourceSystem == sourceSystem);
        }

        return await query.OrderByDescending(x => x.Timestamp).ToListAsync();
    }
    public async Task<List<IntegrationDetail>> GetDetailsByLogId(Guid logId)
    {
        return await _context.IntegrationDetails.AsNoTracking()
            .Where(d => d.IntegrationLogId == logId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
    public async Task<List<IntegrationItem>> GetItemsByDetailId(Guid detailId)
    {
        return await _context.IntegrationItems.AsNoTracking()
            .Where(i => i.IntegrationDetailId == detailId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
    #endregion

    #region Context
    public IQueryable<IntegrationLog> GetIntegrationLogs()
    {
        return _context.IntegrationLogs;
    }
    public IQueryable<IntegrationDetail> GetIntegrationDetails()
    {
        return _context.IntegrationDetails;
    }
    public IQueryable<IntegrationItem> GetIntegrationItems()
    {
        return _context.IntegrationItems;
    }
    #endregion
}