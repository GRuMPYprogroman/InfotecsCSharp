using Microsoft.AspNetCore.Mvc;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Services;

namespace TimescaleService.Infrastructure.Controllers;

[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    IValuesService _valuesService;

    public ValuesController(IValuesService valuesService)
    {
        _valuesService = valuesService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Timescale>>> GetByFileName([FromQuery] string request)
    {
        IReadOnlyCollection<Timescale> lastTenSortedRecords = await _valuesService.GetLastTenByFileName(request);
        
        return Ok(lastTenSortedRecords);
    }
}