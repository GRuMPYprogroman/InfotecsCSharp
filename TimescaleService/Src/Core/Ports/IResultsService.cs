using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services;

public interface IResultsService
{
    Task<AggregatedResult?> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    Task<AggregatedResult?> GetByFileName(string fileName);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByDate(DateTime date);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValue(double value);
    
    Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTime(int time);
}