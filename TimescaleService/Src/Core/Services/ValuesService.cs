using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Core.Services;

public class ValuesService : IValuesService
{
    private readonly IValuesRepository _valuesRepository;

    public ValuesService(IValuesRepository repository)
    {
        _valuesRepository = repository;
    }

    public async Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales)
    {
        return await _valuesRepository.AddAsync(timescales);
    }

    public async Task<IReadOnlyCollection<Timescale>> GetLastTenByFileName(string fileName)
    {
        IReadOnlyList<Timescale> timescales = await _valuesRepository.GetByFileName(fileName);

        var sorted = timescales
            .OrderBy(x => x.Date)
            .TakeLast(10)
            .ToList();
        
        return sorted;
    }
}