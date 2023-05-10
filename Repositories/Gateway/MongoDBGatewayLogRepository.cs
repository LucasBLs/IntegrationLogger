using IntegrationLogger.Configuration;
using IntegrationLogger.Enums;
using IntegrationLogger.Extensions;
using IntegrationLogger.Models.Gateway;
using IntegrationLogger.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace IntegrationLogger.Repositories.Gateway;

public class MongoDBGatewayLogRepository : IApiGatewayLogRepository
{
    private readonly IMongoCollection<GatewayLog> _apiGatewayLogs;
    private readonly IMongoCollection<GatewayDetail> _apiGatewayDetails;
    public MongoDBGatewayLogRepository(IntegrationLoggerConfiguration config)
    {
        var client = new MongoClient(config.ConnectionString);
        var database = client.GetDatabase(config.MongoDatabaseName);

        _apiGatewayLogs = database.GetCollection<GatewayLog>(nameof(GatewayLog));
        _apiGatewayDetails = database.GetCollection<GatewayDetail>(nameof(GatewayDetail));

        CreateApiGatewayLogIndexes();
        CreateApiGatewayDetailIndexes();
    }

    private void CreateApiGatewayLogIndexes()
    {
        var indexKeysBuilder = Builders<GatewayLog>.IndexKeys;
        var compoundIndexKeys = indexKeysBuilder
                                    .Ascending(l => l.Timestamp)
                                    .Ascending(l => l.ProjectName)
                                    .Ascending(l => new
                                    {
                                        l.Timestamp,
                                        l.ProjectName
                                    });
        var indexOptions = new CreateIndexOptions { Name = "ApiGatewayLogIndex" };
        _apiGatewayLogs.Indexes.CreateOne(new CreateIndexModel<GatewayLog>(compoundIndexKeys, indexOptions));
    }
    private void CreateApiGatewayDetailIndexes()
    {
        var indexKeysBuilder = Builders<GatewayDetail>.IndexKeys;
        var apiGatewayLogIdIndexKeys = indexKeysBuilder
                                            .Ascending(i => i.Timestamp)
                                            .Ascending(d => d.ApiGatewayLogId)
                                            .Ascending(o => new
                                            {
                                                o.Timestamp,
                                                o.ApiGatewayLogId
                                            });
        var indexOptions = new CreateIndexOptions { Name = "ApiGatewayDetailIndex" };
        _apiGatewayDetails.Indexes.CreateOne(new CreateIndexModel<GatewayDetail>(apiGatewayLogIdIndexKeys, indexOptions));
    }

    #region AddLogs
    public GatewayLog AddGatewayLog(string projectName, string requestPath, string httpMethod, string clientIp, int statusCode, long requestDuration)
    {
        GatewayLog log = new()
        {
            ProjectName = projectName,
            RequestPath = requestPath,
            HttpMethod = httpMethod,
            ClientIp = clientIp,
            StatusCode = statusCode,
            RequestDuration = requestDuration,
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
            Details = new List<GatewayDetail>(),
        };
        _apiGatewayLogs.InsertOne(log);
        return log;
    }
    public GatewayDetail AddGatewayDetail(GatewayLog log, DetailType type, string? message, object? content, int? GatewayLogStatusCode = null)
    {
        IntegrationStatus statusCode = 0;
        if (GatewayLogStatusCode != null)
        {
            log.StatusCode = GatewayLogStatusCode.Value;

            // Atualizar o documento existente no MongoDB
            var filter = Builders<GatewayLog>.Filter.Eq(d => d.Id, log.Id);
            var update = Builders<GatewayLog>.Update.Set(d => d.StatusCode, GatewayLogStatusCode);
            _apiGatewayLogs.UpdateOne(filter, update);

            statusCode = GatewayLogStatusCode switch
            {
                >= 200 and <= 299 => IntegrationStatus.Success,
                _ => IntegrationStatus.Failed,
            };
        }

        GatewayDetail detail = new()
        {
            ApiGatewayLogId = log.Id,
            Type = type,
            Status = statusCode,
            Message = message,
            Content = content.SerializeIndentedObject(),
            Timestamp = DateTimeOffset.UtcNow.ToLocalTime(),
        };
        log?.Details?.Add(detail);
        _apiGatewayDetails.InsertOne(detail);
        return detail;
    }
    #endregion

    #region GetLogs
    public async Task<(List<GatewayLog> logs, int totalCount)> GetGatewayLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? projectName = null, string? requestPath = null, string? httpMethod = null, string? clientIp = null, int? statusCode = null, int pageIndex = 1, int pageSize = 0)
    {
        var query = _apiGatewayLogs.AsQueryable();

        if (startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(projectName))
            query = query.Where(x => x.ProjectName != null && x.ProjectName == projectName);

        if (!string.IsNullOrEmpty(requestPath))
            query = query.Where(x => x.RequestPath != null && x.RequestPath == requestPath);

        if (!string.IsNullOrWhiteSpace(httpMethod))
            query = query.Where(x => x.HttpMethod != null && x.HttpMethod == httpMethod);

        if (!string.IsNullOrWhiteSpace(clientIp))
            query = query.Where(x => x.ClientIp != null && x.ClientIp == clientIp);

        if (statusCode.HasValue)
            query = query.Where(x => x.StatusCode == statusCode.Value);

        int totalCount = await query.CountAsync();
        if (pageSize > 0)
            query = query.OrderByDescending(x => x.Timestamp).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var logs = await query.ToListAsync();

        return (logs, totalCount);
    }
    public Task<List<GatewayDetail>> GetGatewayDetailsByLogId(Guid apiGatewayLogId)
    {
        return _apiGatewayDetails.AsQueryable().Where(x => x.ApiGatewayLogId == apiGatewayLogId).ToListAsync();
    }
    #endregion
}