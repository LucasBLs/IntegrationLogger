using IntegrationLogger.Models;

namespace IntegrationLogger.Interfaces;

public interface IApiGatewayLog
{
    ApiGatewayLog AddLog(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, long? requestDuration);
    ApiGatewayDetail AddDetail(ApiGatewayLog log, string headerName, string headerValue);
    Task<List<ApiGatewayLog>> SearchLogs(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, DateTimeOffset? startDate, DateTimeOffset? endDate);
    Task<List<ApiGatewayDetail>> GetDetailsByLogId(Guid logId);
}