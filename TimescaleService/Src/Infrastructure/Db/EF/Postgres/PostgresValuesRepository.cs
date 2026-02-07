using Microsoft.EntityFrameworkCore;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Infrastructure.Db.EF.Postgres;

public class PostgresValuesRepository : IValuesRepository
{
    private readonly TimescaleContext _dbContext;

    public PostgresValuesRepository(TimescaleContext context)
    {
        _dbContext = context;
    }
    
    public async Task<Timescale> AddAsync(IReadOnlyCollection<Timescale> timescales)
    {
        await _dbContext.Values.AddRangeAsync(timescales);
        await _dbContext.SaveChangesAsync();
        
        return timescales.Single();
    }

    public async Task<IReadOnlyList<Timescale>> GetByFileName(string FileName)
    {
        var timescales = await _dbContext.Values.Where(x => x.FileName == FileName).ToListAsync();
        
        return timescales;
    }
}