using IntegrationLogger.Extensions;
using IntegrationLogger.Models;
using IntegrationLogger.Models.Gateway;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayLogController : ControllerBase
{
    [HttpGet("get-logs")]
    public async Task<IActionResult> GetLogs(
        [FromServices] IApiGatewayLogRepository repository,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        string? projectName = null,
        string? requestPath = null,
        string? httpMethod = null,
        string? clientIp = null,
        int? statusCode = null,
        int pageIndex = 1,
        int pageSize = 0)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetErrors();
            var response = new ApiResponse<List<GatewayLog>>(false, "ModelState Inválido.", null, errors);
            return BadRequest(response);
        }

        HttpContext.Items["ProjectName"] = "IntegratorLogger";
        try
        {
            return BadRequest();
            var result = await repository.GetGatewayLogs(startDate, endDate, projectName, requestPath, httpMethod, clientIp, statusCode, pageIndex, pageSize);
            return Ok(result.logs);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<GatewayLog>>(false, ex.Message, null, ex.ToString());
            return StatusCode(500, response);
        }
    }

    [HttpGet("get-details")]
    public async Task<IActionResult> GetDetails(
        [FromServices] IApiGatewayLogRepository repository,
        Guid apiGatewayLogId)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetErrors();
            var response = new ApiResponse<List<GatewayDetail>>(false, "ModelState Inválido.", null, errors);
            return BadRequest(response);
        }

        HttpContext.Items["ProjectName"] = "IntegratorLoggera";
        try
        {  
            var result = await repository.GetGatewayDetailsByLogId(apiGatewayLogId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<GatewayDetail>>(false, ex.Message, null, ex.ToString());
            return StatusCode(500, response);
        }
    }
}
