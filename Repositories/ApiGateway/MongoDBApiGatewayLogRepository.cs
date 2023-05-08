using IntegrationLogger.Models.ApiGateway;
using IntegrationLogger.Repositories.Interfaces;

namespace IntegrationLogger.Repositories.ApiGateway;

public class MongoDBApiGatewayLogRepository : IApiGatewayLogRepository
{
    public ApiGatewayDetail AddGatewayDetail(ApiGatewayLog log, string headerName, string headerValue)
    {
        throw new NotImplementedException();
    }

    public ApiGatewayLog AddGatewayLog(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, long? requestDuration)
    {
        throw new NotImplementedException();
    }

    public Task<List<ApiGatewayDetail>> GetGatewayDetailsByLogId(Guid logId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ApiGatewayLog>> SearchGatewayLogs(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        throw new NotImplementedException();
    }
}