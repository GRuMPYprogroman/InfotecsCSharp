using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Repositories;

public interface IResultsRepository
{
    Task<AggregatedResult> AddAsync(AggregatedResult aggregatedResult);
    
    Task<AggregatedResult?> GetByFileNameAsync(string FileName);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByMinimumDateAsync(DateTime MinimumDate);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValueAsync(double AverageValue);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTimeAsync(double AverageExecTime);
    
    Task<IReadOnlyList<AggregatedResult>> GetByMinimumDateRangeAsync(
        DateTime? from,
        DateTime? to);

    Task<IReadOnlyList<AggregatedResult>> GetByAverageValueRangeAsync(
        double? min,
        double? max);

    Task<IReadOnlyList<AggregatedResult>> GetByAverageExecTimeRangeAsync(
        double? min,
        double? max);
    
    Task<AggregatedResult?> UpdateAsync(AggregatedResult timescale);
}