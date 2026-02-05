namespace TimescaleService.Core.Domain;

public class AggregatedResult
{
    public int DateDelta { get; init; }
    
    public DateTime MinimumDate { get; init; }
    
    public double AverageExecTime { get; init; }
    
    public double AverageValue { get; init; }
    
    public double MedianValue { get; init; }
    
    public double MaximumValue { get; init; }
    
    public double MinimumValue { get; init; }
}