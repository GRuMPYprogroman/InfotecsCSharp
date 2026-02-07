using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Repositories;

public interface IResultsRepository
{
    public Task<AggregatedResult> AddAsync(AggregatedResult aggregatedResult);
    
    public Task<AggregatedResult?> GetByFileNameAsync(string FileName);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByMinimumDateAsync(DateTime MinimumDate);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValueAsync(double AverageValue);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTimeAsync(double AverageExecTime);
    
    Task<IReadOnlyList<AggregatedResult>> GetByMinimumDateRangeAsync(
        DateTime? from,
        DateTime? to);

    Task<IReadOnlyList<AggregatedResult>> GetByAverageValueRangeAsync(
        double? min,
        double? max);

    Task<IReadOnlyList<AggregatedResult>> GetByAverageExecTimeRangeAsync(
        double? min,
        double? max);
    
    public Task<AggregatedResult?> UpdateAsync(AggregatedResult timescale);
}