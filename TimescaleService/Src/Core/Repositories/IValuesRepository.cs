using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Repositories;

public interface IValuesRepository
{
    public Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    public Task<IReadOnlyList<Timescale>> GetByFileName(string FileName);
}