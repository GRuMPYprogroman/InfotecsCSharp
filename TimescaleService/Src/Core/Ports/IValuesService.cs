using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services;

public interface IValuesService
{
    Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    Task<IReadOnlyCollection<Timescale>> GetLastTenByFileName(string fileName);
}