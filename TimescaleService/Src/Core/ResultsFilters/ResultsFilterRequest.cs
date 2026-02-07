namespace TimescaleService.Core.ResultsFilters;

public class ResultsFilterRequest
{
    public string? FileName { get; init; }

    public DateTime? MinimumDateFrom { get; init; }
    public DateTime? MinimumDateTo { get; init; }

    public double? AverageValueMin { get; init; }
    public double? AverageValueMax { get; init; }

    public double? AverageExecTimeMin { get; init; }
    public double? AverageExecTimeMax { get; init; }
}