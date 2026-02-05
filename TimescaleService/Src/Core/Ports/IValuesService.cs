using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services;

public interface IValuesService
{
    public Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    public Task<IReadOnlyCollection<Timescale>> GetLastTenByFileName(string fileName);
}