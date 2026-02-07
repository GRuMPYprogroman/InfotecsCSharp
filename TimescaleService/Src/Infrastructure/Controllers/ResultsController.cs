using Microsoft.AspNetCore.Mvc;
using TimescaleService.Core.Domain;
using TimescaleService.Core.ResultsFilters;
using TimescaleService.Core.Services;

namespace TimescaleService.Infrastructure.Controllers;

[ApiController]
[Route("[controller]")]
public class ResultsController : ControllerBase
{
    private readonly ResultsFilterPipeline _pipeline;

    public ResultsController(ResultsFilterPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AggregatedResult>>> GetFiltetedData([FromQuery] ResultsFilterRequest request)
    {
        if (request.MinimumDateFrom.HasValue && request.MinimumDateTo.HasValue && request.MinimumDateFrom > request.MinimumDateTo)
            return BadRequest("MinimumDateFrom must be <= MinimumDateTo");

        if (request.AverageValueMin.HasValue && request.AverageValueMax.HasValue && request.AverageValueMin > request.AverageValueMax)
            return BadRequest("AverageValueMin must be <= AverageValueMax");

        if (request.AverageExecTimeMin.HasValue && request.AverageExecTimeMax.HasValue && request.AverageExecTimeMin > request.AverageExecTimeMax)
            return BadRequest("AverageExecTimeMin must be <= AverageExecTimeMax");

        
        var result = await _pipeline.ExecuteAsync(request);
        
        return Ok(result);
    }
}