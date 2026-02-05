using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Repositories;

public interface IResultsRepository
{
    public Task<AggregatedResult> AddAsync(AggregatedResult aggregatedResult);
    
    public Task<AggregatedResult?> GetByFileNameAsync(string FileName);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByMinimumDateAsync(DateTime MinimumDate);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValueAsync(double AverageValue);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTimeAsync(int AverageExecTime); 
    
    public Task<AggregatedResult> UpdateAsync(AggregatedResult timescale);
}