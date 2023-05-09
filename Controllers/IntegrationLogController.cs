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
        {
            var errors = ModelState.GetErrors();
            var response = new ApiResponse<List<IntegrationLog>>(false, "ModelState Inválido.", null, errors);
            return BadRequest(response);
        }

        try
        {
            var result = await repository.GetLogs(startDate, endDate, integrationName, externalSystem, sourceSystem, groupByIntegrationName, pageIndex, pageSize);
            return Ok(result.logs);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<IntegrationLog>>(false, ex.Message, null, ex.ToString());
            return StatusCode(500, response);
        }
    }

    [HttpGet("get-details")]
    public async Task<IActionResult> GetDetails(
        [FromServices] IIntegrationLogRepository repository,
        DateTimeOffset? startDate = null, 
        DateTimeOffset? endDate = null, 
        string? detailIdentifier = null, 
        bool filterWithIdentifier = true, 
        string? integrationName = null, 
        Guid? integrationLogId = null, 
        int pageIndex = 1, 
        int pageSize = 0)
    {
        if(!ModelState.IsValid)
        {
            var errors = ModelState.GetErrors();
            var response = new ApiResponse<List<IntegrationDetail>>(false, "ModelState Inválido.", null, errors);
            return BadRequest(response);
        }

        try
        {
            var result = await repository.GetDetails(startDate, endDate, detailIdentifier, filterWithIdentifier, integrationName, integrationLogId, pageIndex, pageSize);
            return Ok(result.details);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<IntegrationDetail>>(false, ex.Message, null, ex.ToString());
            return StatusCode(500, response);
        }
    }

    [HttpGet("get-items")]
    public async Task<IActionResult> GetItems(
               [FromServices] IIntegrationLogRepository repository,
               Guid detailId)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.GetErrors();
            var response = new ApiResponse<List<IntegrationItem>>(false, "ModelState Inválido.", null, errors);
            return BadRequest(response);
        }
        try
        {
            var result = await repository.GetItemsByDetailId(detailId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<List<IntegrationItem>>(false, ex.Message, null, ex.ToString());
            return StatusCode(500, response);
        }
    }
}