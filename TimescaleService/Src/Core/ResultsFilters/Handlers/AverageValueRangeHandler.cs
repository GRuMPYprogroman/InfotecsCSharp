using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public class AverageValueRangeHandler : BaseResultsHandler
{
    private readonly IResultsRepository _repo;

    public AverageValueRangeHandler(IResultsRepository repo) => _repo = repo;

    public override async Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
    {
        if (request.AverageValueMin.HasValue || request.AverageValueMax.HasValue)
        {
            if (!ctx.HasData)
            {
                var fromDb = await _repo.GetByAverageValueRangeAsync(request.AverageValueMin, request.AverageValueMax);
                
                ctx.SetItems(fromDb);
            }
            else
            {
                var min = request.AverageValueMin;
                var max = request.AverageValueMax;

                ctx.SetItems(ctx.Items.Where(x =>
                    (!min.HasValue || x.AverageValue >= min.Value) &&
                    (!max.HasValue || x.AverageValue <= max.Value)));
            }
        }

        return await base.HandleAsync(request, ctx);
    }
}