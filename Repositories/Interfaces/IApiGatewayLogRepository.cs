using IntegrationLogger.Models.ApiGateway;

namespace IntegrationLogger.Repositories.Interfaces;

public interface IApiGatewayLogRepository
{
    ApiGatewayLog AddGatewayLog(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, long? requestDuration);
    ApiGatewayDetail AddGatewayDetail(ApiGatewayLog log, string headerName, string headerValue);
    Task<List<ApiGatewayLog>> SearchGatewayLogs(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<List<ApiGatewayDetail>> GetGatewayDetailsByLogId(Guid logId);
}