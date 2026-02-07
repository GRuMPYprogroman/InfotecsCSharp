using TimescaleService.Core.Domain;

namespace WebApiTests;

public class TimescaleTests
{
    [Fact]
    public void Ctor_Should_Set_Properties()
    {
        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        var ts = new Timescale("a.csv", dt, 10, 1.5);

        Assert.Equal("a.csv", ts.FileName);
        Assert.Equal(dt, ts.Date);
        Assert.Equal(10, ts.ExecutionTime);
        Assert.Equal(1.5, ts.Value);
    }

    [Fact]
    public void Ctor_Should_Throw_When_Date_Is_Not_Utc()
    {
        var dtLocal = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Local);

        Assert.Throws<ArgumentException>(() => new Timescale("a.csv", dtLocal, 10, 1.0));
    }

    [Fact]
    public void Ctor_Should_Throw_When_Date_Is_Too_Early()
    {
        var dt = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        Assert.Throws<ArgumentOutOfRangeException>(() => new Timescale("a.csv", dt, 10, 1.0));
    }

    [Fact]
    public void Ctor_Should_Throw_When_ExecutionTime_Is_Negative()
    {
        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        Assert.Throws<ArgumentOutOfRangeException>(() => new Timescale("a.csv", dt, -1, 1.0));
    }

    [Fact]
    public void Ctor_Should_Throw_When_Value_Is_Negative()
    {
        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        Assert.Throws<ArgumentOutOfRangeException>(() => new Timescale("a.csv", dt, 1, -0.01));
    }
}