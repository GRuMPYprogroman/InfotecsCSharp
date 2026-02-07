using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Repositories;

public interface IValuesRepository
{
    Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales);
    
    Task<IReadOnlyList<Timescale>> GetByFileName(string FileName);
    
    Task<Timescale?> UpdateAsync(IReadOnlyCollection<Timescale> timescales);
}