using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public class AverageExecTimeRangeHandler : BaseResultsHandler
{
    private readonly IResultsRepository _repo;

    public AverageExecTimeRangeHandler(IResultsRepository repo) => _repo = repo;

    public override async Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
    {
        if (request.AverageExecTimeMin.HasValue || request.AverageExecTimeMax.HasValue)
        {
            if (!ctx.HasData)
            {
                var fromDb = await _repo.GetByAverageExecTimeRangeAsync(request.AverageExecTimeMin, request.AverageExecTimeMax);
                
                ctx.SetItems(fromDb);
            }
            else
            {
                var min = request.AverageExecTimeMin;
                var max = request.AverageExecTimeMax;

                ctx.SetItems(ctx.Items.Where(x =>
                    (!min.HasValue || x.AverageExecTime >= min.Value) &&
                    (!max.HasValue || x.AverageExecTime <= max.Value)));
            }
        }

        return await base.HandleAsync(request, ctx);
    }
}