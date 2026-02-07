using TimescaleService.Core.Domain;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public abstract class BaseResultsHandler : IResultsFilterHandler
{
    private IResultsFilterHandler _nextHandler;

    public IResultsFilterHandler SetNext(IResultsFilterHandler handler)
    {
        this._nextHandler = handler;

        return handler;
    }
        
    public virtual Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
    {
        if (this._nextHandler != null)
        {
            return this._nextHandler.HandleAsync(request, ctx);
        }
        else
        {
            return null;
        }
    }
}