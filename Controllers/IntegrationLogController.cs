using IntegrationLogger.Extensions;
using IntegrationLogger.Models;
using IntegrationLogger.Models.Integration;
using IntegrationLogger.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegrationLogController : ControllerBase
{
    [HttpGet("get-logs")]
    public async Task<IActionResult> GetLogs(
        [FromServices] IIntegrationLogRepository repository,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null,
        string? integrationName = null,
        string? externalSystem = null,
        string? sourceSystem = null,
        bool groupByIntegrationName = false,
        int pageIndex = 1,
        int pageSize = 0)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<List<IntegrationLog>>(false, "ModelState Inválido.", null, ModelState.GetErrors()));
        try
        {
            var result = await repository.GetLogs(startDate, endDate, integrationName, externalSystem, sourceSystem, groupByIntegrationName, pageIndex, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<IntegrationLog>>(false, ex.Message, null, ex);
            return BadRequest(response);
        }
    }
}