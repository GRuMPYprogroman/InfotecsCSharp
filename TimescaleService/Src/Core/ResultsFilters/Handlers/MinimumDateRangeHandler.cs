using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public class MinimumDateRangeHandler : BaseResultsHandler
{
    private readonly IResultsRepository _repo;

    public MinimumDateRangeHandler(IResultsRepository repo) => _repo = repo;

    public override async Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
    {
        if (request.MinimumDateFrom.HasValue || request.MinimumDateTo.HasValue)
        {
            if (!ctx.HasData)
            {
                var fromDb = await _repo.GetByMinimumDateRangeAsync(request.MinimumDateFrom, request.MinimumDateTo);
                
                ctx.SetItems(fromDb);
            }
            else
            {
                var from = request.MinimumDateFrom;
                var to = request.MinimumDateTo;

                ctx.SetItems(ctx.Items.Where(x =>
                    (!from.HasValue || x.MinimumDate >= from.Value) &&
                    (!to.HasValue || x.MinimumDate <= to.Value)));
            }
        }

        return await base.HandleAsync(request, ctx);
    }
}