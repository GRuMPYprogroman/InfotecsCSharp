using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services;

public interface IResultsService
{
    public Task<AggregatedResult?> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    public Task<AggregatedResult?> GetByFileName(string fileName);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByDate(DateTime date);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValue(double value);
    
    public Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTime(int time);
}