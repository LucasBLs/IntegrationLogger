using IntegrationLogger.Enums;
using IntegrationLogger.Models.Gateway;

namespace IntegrationLogger.Repositories.Interfaces;

public interface IApiGatewayLogRepository
{
    GatewayLog AddGatewayLog(string projectName, string requestPath, string httpMethod, string clientIp, int statusCode, long requestDuration);
    GatewayDetail AddGatewayDetail(GatewayLog log, DetailType type, string? message, object? content, int? GatewayLogStatusCode = null);
    Task<(List<GatewayLog> logs, int totalCount)> GetGatewayLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? projectName = null, string? requestPath = null, string? httpMethod = null, string? clientIp = null, int? statusCode = null, int pageIndex = 1, int pageSize = 0);
    Task<List<GatewayDetail>> GetGatewayDetailsByLogId(Guid apiGatewayLogId);
}