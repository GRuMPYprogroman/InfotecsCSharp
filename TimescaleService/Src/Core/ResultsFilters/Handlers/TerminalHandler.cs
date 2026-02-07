using TimescaleService.Core.Domain;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public class TerminalHandler : BaseResultsHandler
{
    public override Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
        => Task.FromResult(ctx.Items);
}