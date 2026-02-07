using Microsoft.EntityFrameworkCore;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Infrastructure.Db.EF.Postgres;

public class PostgresResultsRepository : IResultsRepository
{
    const double eps = 1e-5;

    private readonly TimescaleContext _dbContext;

    public PostgresResultsRepository(TimescaleContext context)
    {
        _dbContext = context;
    }
    
    public async Task<AggregatedResult> AddAsync(AggregatedResult aggregatedResult)
    {
        await _dbContext.Results.AddAsync(aggregatedResult);
        await _dbContext.SaveChangesAsync();
        
        return aggregatedResult;
    }

    public async Task<AggregatedResult?> GetByFileNameAsync(string FileName)
    {
        AggregatedResult? aggregatedResult = await _dbContext.Results.FindAsync(FileName);
        
        return aggregatedResult;
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByMinimumDateAsync(DateTime MinimumDate)
    {
        var aggregatedResult = await _dbContext.Results
            .AsNoTracking()
            .Where(x => x.MinimumDate == MinimumDate)
            .ToListAsync();
        
        return aggregatedResult;
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValueAsync(double AverageValue)
    {
        var aggregatedResult = await _dbContext.Results
            .AsNoTracking()
            .Where(x => Math.Abs(x.AverageValue - AverageValue) < eps)
            .ToListAsync();
        
        return aggregatedResult;
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTimeAsync(double AverageExecTime)
    {
        var aggregatedResult = await _dbContext.Results
            .AsNoTracking()
            .Where(x => Math.Abs(x.AverageExecTime - AverageExecTime) < eps)
            .ToListAsync();
        
        return aggregatedResult;
    }
    
    public async Task<IReadOnlyList<AggregatedResult>> GetByMinimumDateRangeAsync(
        DateTime? from,
        DateTime? to)
    {
        IQueryable<AggregatedResult> query = _dbContext.Results.AsNoTracking();

        if (from.HasValue)
            query = query.Where(x => x.MinimumDate >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.MinimumDate <= to.Value);

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<AggregatedResult>> GetByAverageValueRangeAsync(
        double? min,
        double? max)
    {
        IQueryable<AggregatedResult> query = _dbContext.Results.AsNoTracking();

        if (min.HasValue)
            query = query.Where(x => x.AverageValue >= min.Value);

        if (max.HasValue)
            query = query.Where(x => x.AverageValue <= max.Value);

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<AggregatedResult>> GetByAverageExecTimeRangeAsync(
        double? min,
        double? max)
    {
        IQueryable<AggregatedResult> query = _dbContext.Results.AsNoTracking();

        if (min.HasValue)
            query = query.Where(x => x.AverageExecTime >= min.Value);

        if (max.HasValue)
            query = query.Where(x => x.AverageExecTime <= max.Value);

        return await query.ToListAsync();
    }

    public async Task<AggregatedResult?> UpdateAsync(AggregatedResult timescale)
    {
        AggregatedResult? result = await _dbContext.Results.FindAsync(timescale.FileName);

        if (result != null)
        {
            result.DateDelta = timescale.DateDelta;
            result.MinimumDate = timescale.MinimumDate;
            result.AverageExecTime = timescale.AverageExecTime;
            result.AverageValue = timescale.AverageValue;
            result.MedianValue = timescale.MedianValue;
            result.MinimumValue = timescale.MinimumValue;
            result.MaximumValue = timescale.MaximumValue;
            
            await _dbContext.SaveChangesAsync();
        }

        return result;
    }
}