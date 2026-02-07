using TimescaleService.Core.Domain;
using TimescaleService.Core.ResultsFilters.Handlers;

namespace TimescaleService.Core.ResultsFilters;

public class ResultsFilterPipeline : IResultsFilterPipeline
{
    private readonly FileNameHandler _fileName;
    private readonly MinimumDateRangeHandler _minDate;
    private readonly AverageValueRangeHandler _avgValue;
    private readonly AverageExecTimeRangeHandler _avgExec;
    private readonly TerminalHandler _terminal;

    public ResultsFilterPipeline(
        FileNameHandler fileName,
        MinimumDateRangeHandler minDate,
        AverageValueRangeHandler avgValue,
        AverageExecTimeRangeHandler avgExec,
        TerminalHandler terminal)
    {
        _fileName = fileName;
        _minDate = minDate;
        _avgValue = avgValue;
        _avgExec = avgExec;
        _terminal = terminal;

        _fileName
            .SetNext(_minDate)
            .SetNext(_avgValue)
            .SetNext(_avgExec)
            .SetNext(_terminal);
    }

    public Task<List<AggregatedResult>> ExecuteAsync(ResultsFilterRequest request)
    {
        var ctx = new ResultsFilterContext();
        
        return _fileName.HandleAsync(request, ctx);
    }
}