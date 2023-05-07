using IntegrationLogger.Configuration;
using IntegrationLogger.Enums;
using IntegrationLogger.Interfaces;
using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace IntegrationLogger.Services.Integration;
public class MongoDBIntegrationLogService : IIntegrationLogService, IIntegrationLogQueryable
{
    private readonly IMongoCollection<IntegrationLog> _integrationLogs;
    private readonly IMongoCollection<IntegrationDetail> _integrationDetails;
    private readonly IMongoCollection<IntegrationItem> _integrationItems;

    public MongoDBIntegrationLogService(IntegrationLoggerConfiguration config)
    {
        var client = new MongoClient(config.ConnectionString);
        var database = client.GetDatabase(config.MongoDatabaseName);

        _integrationLogs = database.GetCollection<IntegrationLog>("IntegrationLogs");
        _integrationDetails = database.GetCollection<IntegrationDetail>("IntegrationDetails");
        _integrationItems = database.GetCollection<IntegrationItem>("IntegrationItems");

        CreateIntegrationLogIndexes();
        CreateIntegrationDetailIndexes();
        CreateIntegrationItemIndexes();
    }

    private void CreateIntegrationLogIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationLog>.IndexKeys;
        var compoundIndexKeys = indexKeysBuilder
                                    .Ascending(l => l.Timestamp)
                                    .Ascending(l => l.IntegrationName)
                                    .Ascending(l => l.ExternalSystem)
                                    .Ascending(l => l.SourceSystem);
        var indexOptions = new CreateIndexOptions { Name = "CompoundIndex" };
        _integrationLogs.Indexes.CreateOne(new CreateIndexModel<IntegrationLog>(compoundIndexKeys, indexOptions));

    }
    private void CreateIntegrationDetailIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationDetail>.IndexKeys;
        var integrationLogIdIndexKeys = indexKeysBuilder
                                            .Ascending(d => d.IntegrationLogId)
                                            .Ascending(i => i.Timestamp);
        var indexOptions = new CreateIndexOptions { Name = "IntegrationLogIdIndex" };
        _integrationDetails.Indexes.CreateOne(new CreateIndexModel<IntegrationDetail>(integrationLogIdIndexKeys, indexOptions));
    }
    private void CreateIntegrationItemIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationItem>.IndexKeys;
        var integrationDetailIdIndexKeys = indexKeysBuilder
                                            .Ascending(i => i.IntegrationDetailId)
                                            .Ascending(i => i.Timestamp);
        var indexOptions = new CreateIndexOptions { Name = "IntegrationDetailIdIndex" };
        _integrationItems.Indexes.CreateOne(new CreateIndexModel<IntegrationItem>(integrationDetailIdIndexKeys, indexOptions));
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

        _integrationLogs.InsertOne(log);
        return log;
    }
    public IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem)
    {
        return Log(integrationName, message, externalSystem, sourceSystem);
    }
    public IntegrationDetail AddDetail(IntegrationLog log, IntegrationStatus status, string? detailIdentifier, string? message)
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

        _integrationDetails.InsertOne(detail);
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

            // Atualizar o documento existente no MongoDB
            var filter = Builders<IntegrationDetail>.Filter.Eq(d => d.Id, detail.Id);
            var update = Builders<IntegrationDetail>.Update.Set(d => d.Status, detail.Status);
            _integrationDetails.UpdateOne(filter, update);
        }

        _integrationItems.InsertOne(item);
        return item;
    }

    public async Task<List<IntegrationLog>> SearchLogs(DateTimeOffset startDate, DateTimeOffset endDate, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null)
    {
        var query = _integrationLogs.AsQueryable().Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

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
        return await _integrationDetails.AsQueryable()
            .Where(d => d.IntegrationLogId == logId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
    public async Task<List<IntegrationItem>> GetItemsByDetailId(Guid detailId)
    {
        return await _integrationItems.AsQueryable()
            .Where(i => i.IntegrationDetailId == detailId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }

    public IQueryable<IntegrationLog> GetIntegrationLogs()
    {
        return _integrationLogs.AsQueryable();
    }
    public IQueryable<IntegrationDetail> GetIntegrationDetails()
    {
        return _integrationDetails.AsQueryable();
    }
    public IQueryable<IntegrationItem> GetIntegrationItems()
    {
        return _integrationItems.AsQueryable();
    }
}