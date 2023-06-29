using IntegrationLogger.Configuration;
using IntegrationLogger.Enums;
using IntegrationLogger.Extensions;
using IntegrationLogger.Models.Configuration;
using IntegrationLogger.Models.Integration;
using IntegrationLogger.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IntegrationLogger.Repositories.Integration;
public class MongoDBIntegrationLogRepository : IIntegrationLogRepository
{
    private readonly IMongoCollection<IntegrationLog> _integrationLogs;
    private readonly IMongoCollection<IntegrationDetail> _integrationDetails;
    private readonly IMongoCollection<IntegrationItem> _integrationItems;
    private readonly IMongoCollection<LogConfiguration> _logConfigurations;
    private readonly IMongoCollection<EmailConfiguration> _emailConfigurations;

    public MongoDBIntegrationLogRepository(IntegrationLoggerConfiguration config)
    {
        var client = new MongoClient(config.ConnectionString);
        var database = client.GetDatabase(config.MongoDatabaseName);

        _integrationLogs = database.GetCollection<IntegrationLog>("IntegrationLogs");
        _integrationDetails = database.GetCollection<IntegrationDetail>("IntegrationDetails");
        _integrationItems = database.GetCollection<IntegrationItem>("IntegrationItems");
        _logConfigurations = database.GetCollection<LogConfiguration>("LogConfigurations");
        _emailConfigurations = database.GetCollection<EmailConfiguration>("EmailConfigurations");

        CreateIntegrationLogIndexes();
        CreateIntegrationDetailIndexes();
        CreateIntegrationItemIndexes();
        CreateLogConfigurationIndexes();
        CreateEmailConfigurationIndexes();
    }

    private void CreateIntegrationLogIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationLog>.IndexKeys;
        var compoundIndexKeys = indexKeysBuilder
                                    .Ascending(l => l.Timestamp)
                                    .Ascending(l => l.IntegrationName);
        var indexOptions = new CreateIndexOptions { Name = "IntegrationLogIndex" };
        _integrationLogs.Indexes.CreateOne(new CreateIndexModel<IntegrationLog>(compoundIndexKeys, indexOptions));

    }
    private void CreateIntegrationDetailIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationDetail>.IndexKeys;
        var integrationLogIdIndexKeys = indexKeysBuilder
                                            .Ascending(i => i.Timestamp)
                                            .Ascending(d => d.IntegrationLogId)
                                            .Ascending(x => x.DetailIdentifier);
        var indexOptions = new CreateIndexOptions { Name = "IntegrationDetailIndex" };
        _integrationDetails.Indexes.CreateOne(new CreateIndexModel<IntegrationDetail>(integrationLogIdIndexKeys, indexOptions));
    }
    private void CreateIntegrationItemIndexes()
    {
        var indexKeysBuilder = Builders<IntegrationItem>.IndexKeys;
        var integrationDetailIdIndexKeys = indexKeysBuilder
                                            .Ascending(i => i.Timestamp)
                                            .Ascending(i => i.IntegrationDetailId);                                       
        var indexOptions = new CreateIndexOptions { Name = "IntegrationItemIndex" };
        _integrationItems.Indexes.CreateOne(new CreateIndexModel<IntegrationItem>(integrationDetailIdIndexKeys, indexOptions));
    }
    private void CreateLogConfigurationIndexes()
    {
        var indexKeysBuilder = Builders<LogConfiguration>.IndexKeys;
        var compoundIndexKeys = indexKeysBuilder
                                    .Ascending(l => l.LogLevel);
        var indexOptions = new CreateIndexOptions { Name = "LogConfigurationIndex" };
        _logConfigurations.Indexes.CreateOne(new CreateIndexModel<LogConfiguration>(compoundIndexKeys, indexOptions));
    }
    private void CreateEmailConfigurationIndexes()
    {
        var indexKeysBuilder = Builders<EmailConfiguration>.IndexKeys;
        var compoundIndexKeys = indexKeysBuilder
                                    .Ascending(l => l.SenderEmail)
                                    .Ascending(l => l.RecipientEmail);
        var indexOptions = new CreateIndexOptions { Name = "EmailConfigurationIndex" };
        _emailConfigurations.Indexes.CreateOne(new CreateIndexModel<EmailConfiguration>(compoundIndexKeys, indexOptions));
    }

    #region AddLogs
    public IntegrationLog AddLog(string integrationName, string message, string externalSystem, string sourceSystem)
    {
        LogLevel configuredLogLevel = _logConfigurations.Find(new BsonDocument()).FirstOrDefault()?.LogLevel ?? LogLevel.Alert;
        if (configuredLogLevel == LogLevel.None)
            return new();

        var project = _integrationLogs.Find(log => log.IntegrationName == integrationName).FirstOrDefault();
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

        _integrationLogs.InsertOne(log);
        return log;
    }
    public IntegrationDetail AddDetail(IntegrationLog log, LogLevel status, string? detailIdentifier, string? message)
    {
        LogLevel configuredLogLevel = _logConfigurations.Find(new BsonDocument()).FirstOrDefault()?.LogLevel ?? LogLevel.Alert;
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

        _integrationDetails.InsertOne(detail);
        return detail;
    }
    public IntegrationItem AddItem(IntegrationDetail detail, string itemIdentifier, LogLevel itemStatus, string? message, object? content)
    {
        LogLevel configuredLogLevel = _logConfigurations.Find(new BsonDocument()).FirstOrDefault()?.LogLevel ?? LogLevel.Alert;
        if (configuredLogLevel == LogLevel.None)
            return new();
        IntegrationItem item = new()
        {
            IntegrationDetailId = detail.Id,
            ItemIdentifier = itemIdentifier,
            ItemStatus = itemStatus,
            Message = message,
            Content = content.SerializeIndentedObject(),
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
        };

        if (item.ItemStatus == LogLevel.Failed)
        {
            detail.Status = LogLevel.Failed;

            // Atualizar o documento existente no MongoDB
            var filter = Builders<IntegrationDetail>.Filter.Eq(d => d.Id, detail.Id);
            var update = Builders<IntegrationDetail>.Update.Set(d => d.Status, detail.Status);
            _integrationDetails.UpdateOne(filter, update);
        }

        _integrationItems.InsertOne(item);
        return item;
    }
    #endregion

    #region GetLogs
    public async Task<(List<IntegrationLog> logs, int totalCount)> GetLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? integrationName = null, string? externalSystem = null, string? sourceSystem = null, bool groupByIntegrationName = false, int pageIndex = 1, int pageSize = 0)
    {
        var query = _integrationLogs.AsQueryable();

        if (startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(integrationName))
            query = query.Where(l => l.IntegrationName != null && l.IntegrationName.ToLower().Contains(integrationName.ToLower()));

        if (!string.IsNullOrEmpty(externalSystem))
            query = query.Where(l => l.ExternalSystem != null && l.ExternalSystem.Contains(externalSystem));

        if (!string.IsNullOrEmpty(sourceSystem))
            query = query.Where(l => l.SourceSystem != null && l.SourceSystem.Contains(sourceSystem));

        if (groupByIntegrationName)
            query = query.GroupBy(log => log.IntegrationName)
                         .Select(group => group.OrderByDescending(log => log.Timestamp).First());

        int totalCount = await query.CountAsync();

        if (pageSize > 0)
            query = query.OrderByDescending(x => x.Timestamp).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var details = await query.ToListAsync();

        return (details, totalCount);
    }
    public async Task<(List<IntegrationDetail> details, int totalCount)> GetDetails(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? detailIdentifier = null, bool filterWithIdentifier = true, string? integrationName = null, Guid? integrationLogId = null, int pageIndex = 1, int pageSize = 0)
    {
        var query = _integrationDetails.AsQueryable();

        if (integrationLogId != null)
            query = query.Where(l => l.IntegrationLogId == integrationLogId);

        if (startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(detailIdentifier))
            query = query.Where(l => l.DetailIdentifier != null && l.DetailIdentifier.ToLower().Contains(detailIdentifier.ToLower()));

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
        return await _integrationItems.AsQueryable()
            .Where(i => i.IntegrationDetailId == detailId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
    #endregion
}