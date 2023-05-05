using IntegrationLogger.Data;
using IntegrationLogger.Interfaces;
using IntegrationLogger.Models;

namespace IntegrationLogger.Services.ApiGateway;

public class RelactionalApiGatewayLogService : IApiGatewayLogService, IIntegrationLogQueryable
{
    private readonly IntegrationLogContextBase _context;

    public RelactionalApiGatewayLogService(IntegrationLogContextBase context)
    {
        _context = context;
    }

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

    public IQueryable<IntegrationDetail> GetIntegrationDetails()
    {
        throw new NotImplementedException();
    }

    public IQueryable<IntegrationItem> GetIntegrationItems()
    {
        throw new NotImplementedException();
    }

    public IQueryable<IntegrationLog> GetIntegrationLogs()
    {
        throw new NotImplementedException();
    }

    public Task<List<ApiGatewayLog>> SearchGatewayLogs(string? projectName, string? requestPath, string? httpMethod, string? clientIp, int? statusCode, DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        throw new NotImplementedException();
    }
}