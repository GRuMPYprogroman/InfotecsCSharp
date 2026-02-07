using TimescaleService.Core.Domain;

namespace TimescaleService.Core.ResultsFilters;

public interface IResultsFilterPipeline
{
    Task<List<AggregatedResult>> ExecuteAsync(ResultsFilterRequest request);
}