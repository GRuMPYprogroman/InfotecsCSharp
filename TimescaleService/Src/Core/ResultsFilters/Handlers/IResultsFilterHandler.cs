using TimescaleService.Core.Domain;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public interface IResultsFilterHandler
{
    IResultsFilterHandler SetNext(IResultsFilterHandler next);
    
    Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx);
}