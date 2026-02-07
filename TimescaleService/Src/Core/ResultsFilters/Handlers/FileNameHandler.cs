using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;

namespace TimescaleService.Core.ResultsFilters.Handlers;

public class FileNameHandler : BaseResultsHandler
{
    private readonly IResultsRepository _repo;

    public FileNameHandler(IResultsRepository repo) => _repo = repo;

    public override async Task<List<AggregatedResult>> HandleAsync(ResultsFilterRequest request, ResultsFilterContext ctx)
    {
        if (!string.IsNullOrWhiteSpace(request.FileName))
        {
            if (!ctx.HasData)
            {
                AggregatedResult? fromDb = await _repo.GetByFileNameAsync(request.FileName);
                
                if (fromDb != null)
                    ctx.SetItems(new List<AggregatedResult>(){ fromDb });
            }
            else
            {
                ctx.SetItems(ctx.Items.Where(x => x.FileName == request.FileName));
            }
        }

        return await base.HandleAsync(request, ctx);
    }
}