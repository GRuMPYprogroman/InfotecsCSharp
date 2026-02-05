namespace TimescaleService.Core.Domain;

public class Timescale
{
    private readonly DateTime _date;
    private readonly int _executionTime;
    private readonly double _value;

    public Timescale(string fileName, DateTime date, int executionTime, double value)
    {
        FileName = fileName;
        Date = date;
        ExecutionTime = executionTime;
        Value = value;
    }
    
    public string FileName { get;}

    public DateTime Date
    {
        get => _date;

        private init
        {
            if (value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date must be in UTC.", nameof(value));
            
            if (value < new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                || value > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _date = value;
        }
    }

    public int ExecutionTime
    {
        get => _executionTime;

        private init
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _executionTime = value;
        }
    }

    public double Value
    {
        get => _value;

        private init
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _value = value;
        }
    }
}