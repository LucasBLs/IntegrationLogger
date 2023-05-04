using IntegrationLogger.Interfaces;
using IntegrationLogger.Models;
using IntegrationLogger.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Controllers;
[ServiceFilter(typeof(RoleBasedAuthorizationFilter))]
public class LogViewerController : Controller
{
    private readonly IIntegrationLogQueryable _integrationLogQueryable;
 
    public LogViewerController(IIntegrationLogQueryable integrationLogQueryable)
    {
        _integrationLogQueryable = integrationLogQueryable;
    }

    public async Task<IActionResult> Index(string sortLog, string currentFilter, string searchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortLog;

        if (searchString != null)
            pageNumber = 1;
        else
            searchString = currentFilter;

        ViewData["CurrentFilter"] = searchString;
        var integrationLogs = from s in _integrationLogQueryable.GetIntegrationLogs()
                              select s;
        if (!string.IsNullOrEmpty(searchString))
        {
            integrationLogs = integrationLogs.Where(s => s.IntegrationName!.Contains(searchString));
        }

        int pageSize = 10;
        return View(await PaginatedList<IntegrationLog>.CreateAsync(integrationLogs.AsNoTracking().OrderByDescending(x => x.Timestamp), pageNumber ?? 1, pageSize));
    }

    public async Task<IActionResult> LogDetail(Guid? id, string sortLogDetail, string currentFilter, string searchString, int? pageNumber, bool onlyWithIdentifier = true)
    {
        ViewData["CurrentSort"] = sortLogDetail;

        if (searchString != null)
            pageNumber = 1;
        else
            searchString = currentFilter;

        ViewData["CurrentFilter"] = searchString;
        ViewData["IntegrationLogData"] = await _integrationLogQueryable.GetIntegrationLogs().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        var integrationLogsDetails = from s in _integrationLogQueryable.GetIntegrationDetails()
                                     select s;
        int pageSize = 10;

        if (id is null)
            integrationLogsDetails = integrationLogsDetails.AsNoTracking().OrderByDescending(x => x.Timestamp);
        else
            integrationLogsDetails = integrationLogsDetails.AsNoTracking().Where(x => x.IntegrationLogId == id);

        ViewData["OnlyWithIdentifier"] = onlyWithIdentifier;
        if (onlyWithIdentifier)
        {
            integrationLogsDetails = integrationLogsDetails.Where(x => !string.IsNullOrEmpty(x.DetailIdentifier));
        }

        var integrationLogDetailsList = await PaginatedList<IntegrationDetail>.CreateAsync(integrationLogsDetails.OrderByDescending(x => x.Timestamp), pageNumber ?? 1, pageSize);
        var errorStatusDict = new Dictionary<Guid, bool>();
        foreach (var detail in integrationLogDetailsList)
        {
            errorStatusDict[detail.Id] = await _integrationLogQueryable.GetIntegrationItems().AnyAsync(x => x.IntegrationDetailId == detail.Id && x.ItemStatus == Enums.IntegrationStatus.Failed);
        }
        ViewData["ErrorStatusDict"] = errorStatusDict;
      
        return View(integrationLogDetailsList);
    }

    [HttpGet]
    public async Task<IActionResult> GetLogItems(Guid id)
    {
        try
        {
            //var logDetails = (await PaginatedList<IntegrationDetail>.CreateAsync(_integrationLogQueryable.GetIntegrationDetails()
            //        .Where(x => x.Id == id), pagination: false));

            List<IntegrationItem> newLogItems = new();

            //foreach (var detail in logDetails)
            //{
            //    var logItems = (await PaginatedList<IntegrationItem>.CreateAsync(_integrationLogQueryable.GetIntegrationItems()
            //                    .Where(x => x.IntegrationDetailId == detail.Id).OrderByDescending(x => x.Timestamp), pagination: false));
            //    newLogItems.AddRange(logItems);
            //}
            var logItems = (await PaginatedList<IntegrationItem>.CreateAsync(_integrationLogQueryable.GetIntegrationItems()
                               .Where(x => x.IntegrationDetailId == id).OrderByDescending(x => x.Timestamp), pagination: false));
            newLogItems.AddRange(logItems);

            return Json(newLogItems);
        }
        catch (Exception ex)
        {
            // Retorna a mensagem de erro como JSON, incluindo o status 500 para indicar erro no servidor
            return StatusCode(500, new { error = $"Erro ao buscar itens do log: {ex.Message}" });
        }
    }
}