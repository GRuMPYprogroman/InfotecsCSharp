namespace TimescaleService.Core.Domain;

public class AggregatedResult
{
    public string FileName { get; init; }
    
    public double DateDelta { get; set; }
    
    public DateTime MinimumDate { get; set; }
    
    public double AverageExecTime { get; set; }
    
    public double AverageValue { get; set; }
    
    public double MedianValue { get; set; }
    
    public double MaximumValue { get; set; }
    
    public double MinimumValue { get; set; }
}