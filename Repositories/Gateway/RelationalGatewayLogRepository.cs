using IntegrationLogger.Data;
using IntegrationLogger.Enums;
using IntegrationLogger.Extensions;
using IntegrationLogger.Models.Gateway;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Repositories.Gateway;

public class RelationalGatewayLogRepository : IApiGatewayLogRepository
{
    private readonly IntegrationLogContextBase _context;
    public RelationalGatewayLogRepository(IntegrationLogContextBase context)
    {
        _context = context;
    }

    public GatewayLog AddGatewayLog(string projectName, string requestPath, string httpMethod, string clientIp, int statusCode, long requestDuration)
    {
        var log = new GatewayLog
        {
            Id = Guid.NewGuid(),
            ProjectName = projectName,
            RequestPath = requestPath,
            HttpMethod = httpMethod,
            ClientIp = clientIp,
            StatusCode = statusCode,
            RequestDuration = requestDuration,
            Timestamp = DateTimeOffset.Now.ToLocalTime()
        };

        _context.ApiGatewayLogs.Add(log);
        _context.SaveChanges();
        return log;
    }
    public GatewayDetail AddGatewayDetail(GatewayLog log, DetailType type, string? message = null, object? content = null, int? GatewayLogStatusCode = null)
    {
        IntegrationStatus statusCode = 0;
        if (GatewayLogStatusCode != null)
        {
            log.StatusCode = GatewayLogStatusCode.Value;
            _context.ApiGatewayLogs.Update(log);

            statusCode = GatewayLogStatusCode switch
            {
                >= 200 and <= 299 => IntegrationStatus.Success,
                _ => IntegrationStatus.Failed,
            };
        }
      
        var detail = new GatewayDetail
        {
            Id = Guid.NewGuid(),
            Type = type,
            Status = statusCode,
            Message = message,
            Content = content.SerializeIndentedObject(),
            Timestamp = DateTimeOffset.Now.ToLocalTime(),
            ApiGatewayLogId = log.Id
        };

        _context.ApiGatewayDetails.Add(detail);
        _context.SaveChanges();
        return detail;
    }

    public async Task<(List<GatewayLog> logs, int totalCount)> GetGatewayLogs(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? projectName = null, string? requestPath = null, string? httpMethod = null, string? clientIp = null, int? statusCode = null, int pageIndex = 1, int pageSize = 0)
    {
        var query = _context.ApiGatewayLogs.AsQueryable();

        if (startDate != null && endDate != null)
            query = query.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);

        if (!string.IsNullOrEmpty(projectName))
            query = query.Where(x => x.ProjectName != null && x.ProjectName.Contains(projectName));

        if (!string.IsNullOrEmpty(requestPath))
            query = query.Where(x => x.RequestPath != null && x.RequestPath.Contains(requestPath));

        if (!string.IsNullOrEmpty(httpMethod))
            query = query.Where(x => x.HttpMethod != null && x.HttpMethod.Contains(httpMethod));

        if (!string.IsNullOrEmpty(clientIp))
            query = query.Where(x => x.ClientIp != null && x.ClientIp.Contains(clientIp));

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
        return _context.ApiGatewayDetails.Where(x => x.ApiGatewayLogId == apiGatewayLogId).ToListAsync();
    }
}