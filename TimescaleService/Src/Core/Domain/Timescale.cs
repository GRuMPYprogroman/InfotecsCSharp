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
    
    public long Id { get; private set; }
    
    public string FileName { get; private set; }

    public DateTime Date
    {
        get => _date;

        private init
        {
            if (value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date must be in UTC.", nameof(value));
            
            if (value < new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                || value > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException("Date can't be earlier than 2000 or later than today");
            
            _date = value;
        }
    }

    public int ExecutionTime
    {
        get => _executionTime;

        private init
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Time can't be less than 0.");
            
            _executionTime = value;
        }
    }

    public double Value
    {
        get => _value;

        private init
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Value can't be less than 0.");
            
            _value = value;
        }
    }
}