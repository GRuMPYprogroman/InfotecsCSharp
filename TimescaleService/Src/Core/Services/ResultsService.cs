using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;
using TimescaleService.Core.Services.Parser;

namespace TimescaleService.Core.Services;

public class ResultsService : IResultsService
{
    private readonly IResultsRepository _resultsRepository;

    public ResultsService(IResultsRepository repository)
    {
        _resultsRepository = repository;
    }

    public async Task<AggregatedResult?> GetByFileName(string fileName)
    {
        return await _resultsRepository.GetByFileNameAsync(fileName);
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByDate(DateTime date)
    {
        return await _resultsRepository.GetByMinimumDateAsync(date);
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByAverageValue(double value)
    {
        return await _resultsRepository.GetByAverageValueAsync(value);
    }

    public async Task<IReadOnlyCollection<AggregatedResult>> GetByAverageExecTime(int time)
    {
        return await  _resultsRepository.GetByAverageExecTimeAsync(time);
    }

    public async Task<AggregatedResult> AddAsync(IReadOnlyCollection<Timescale> timescales)
    {
        if (timescales == null)
            throw new ArgumentNullException(nameof(timescales));
        
        if (timescales.Count == 0)
            throw new ArgumentException("timescales must have at least one value", nameof(timescales));
        
        AggregatedResult aggregatedResult = AggregateData(timescales);
        
        AggregatedResult? resultDb = await GetByFileName(timescales.First().FileName);

        if (resultDb is null)
        {
            await _resultsRepository.AddAsync(aggregatedResult);
        }
        else
        {
            await _resultsRepository.UpdateAsync(aggregatedResult);
        }   
        
        return aggregatedResult;
    }

    private AggregatedResult AggregateData(IReadOnlyCollection<Timescale> timescales)
    {
        if (timescales == null)
            throw new ArgumentNullException(nameof(timescales));
        
        if (timescales.Count == 0)
            throw new ArgumentException("timescales must have at least one value", nameof(timescales));
        
        DateTime earlierDate = timescales.Min(timeScale => timeScale.Date);
        
        double dateDelta = (timescales.Max(timeScale => timeScale.Date) - earlierDate).TotalSeconds;
        
        double averageExecTime = timescales.Average(timeScale => timeScale.ExecutionTime);
        
        double averageValue = timescales.Average(timeScale => timeScale.Value);
        
        double medianValue = GetMedian(timescales);
        
        double maximumValue = timescales.Max(timeScale => timeScale.Value);
        
        double minimumValue = timescales.Min(timeScale => timeScale.Value);

        var result = new AggregatedResult()
        {
            FileName = timescales.First().FileName,
            MinimumDate = earlierDate,
            DateDelta = dateDelta,
            AverageExecTime = averageExecTime,
            AverageValue = averageValue,
            MedianValue = medianValue,
            MaximumValue = maximumValue,
            MinimumValue = minimumValue,
        };
        
        return result;
    }

    private double GetMedian(IReadOnlyCollection<Timescale> timescales)
    {
        var sortedValues = timescales.Select(timeScale => timeScale.Value).ToArray();
        Array.Sort(sortedValues);

        int size = sortedValues.Length;
        int mid = size / 2;
        
        double median = (size % 2 != 0) ? sortedValues[mid] : (sortedValues[mid] + sortedValues[mid - 1]) / 2;
        
        return median;
    }
}