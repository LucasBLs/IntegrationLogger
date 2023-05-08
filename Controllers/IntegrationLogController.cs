using IntegrationLogger.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationLogger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegrationLogController : ControllerBase
{
    private readonly IIntegrationLogRepository _integrationLogRepository;
    public IntegrationLogController(IIntegrationLogRepository integrationLogRepository)
    {
        _integrationLogRepository = integrationLogRepository;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _integrationLogRepository.GetLogs();
        return Ok(result);
    }
}