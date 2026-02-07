using TimescaleService.Core.Domain;

namespace TimescaleService.Core.ResultsFilters;

public sealed class ResultsFilterContext
{
    public List<AggregatedResult> Items { get; private set; } = new();

    public bool HasData => Items.Count > 0;

    public void SetItems(IEnumerable<AggregatedResult> items)
        => Items = items.ToList();
}