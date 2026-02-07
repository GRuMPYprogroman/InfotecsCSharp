using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;
using TimescaleService.Core.Services;

namespace WebApiTests;

public class ResultsServiceTests
{
    private Timescale Ts(string fn, DateTime dt, int exec, double val) => new(fn, dt, exec, val);

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        var repo = Substitute.For<IResultsRepository>();
        var sut = new ResultsService(repo);

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.AddAsync(null!));
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Empty()
    {
        var repo = Substitute.For<IResultsRepository>();
        var sut = new ResultsService(repo);

        await Assert.ThrowsAsync<ArgumentException>(() => sut.AddAsync(Array.Empty<Timescale>()));
    }

    [Fact]
    public async Task AddAsync_Should_Add_When_No_Existing()
    {
        var repo = Substitute.For<IResultsRepository>();
        var sut = new ResultsService(repo);

        repo.GetByFileNameAsync("a.csv").Returns((AggregatedResult?)null);

        var dt0 = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var dt1 = dt0.AddSeconds(90);

        var input = new[]
        {
            Ts("a.csv", dt0, 10, 1),
            Ts("a.csv", dt1, 20, 3),
        };

        var res = await sut.AddAsync(input);

        await repo.Received(1).AddAsync(Arg.Any<AggregatedResult>());
        await repo.DidNotReceive().UpdateAsync(Arg.Any<AggregatedResult>());

        Assert.Equal("a.csv", res.FileName);
        Assert.Equal(dt0, res.MinimumDate);
        Assert.Equal(15.0, res.AverageExecTime);
        Assert.Equal(2.0, res.AverageValue);
        Assert.Equal(2.0, res.MedianValue);
        Assert.Equal(3.0, res.MaximumValue);
        Assert.Equal(1.0, res.MinimumValue);
        Assert.Equal(90, res.DateDelta);
    }

    [Fact]
    public async Task AddAsync_Should_Update_When_Existing()
    {
        var repo = Substitute.For<IResultsRepository>();
        var sut = new ResultsService(repo);

        repo.GetByFileNameAsync("a.csv").Returns(new AggregatedResult { FileName = "a.csv" });

        var dt0 = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var input = new[] { Ts("a.csv", dt0, 10, 1) };

        var res = await sut.AddAsync(input);

        await repo.Received(1).UpdateAsync(Arg.Any<AggregatedResult>());
        await repo.DidNotReceive().AddAsync(Arg.Any<AggregatedResult>());

        Assert.Equal("a.csv", res.FileName);
    }

    [Fact]
    public async Task GetByFileName_Should_Delegate_To_Repo()
    {
        var repo = Substitute.For<IResultsRepository>();
        var sut = new ResultsService(repo);

        repo.GetByFileNameAsync("a.csv").Returns(new AggregatedResult { FileName = "a.csv" });

        var res = await sut.GetByFileName("a.csv");

        Assert.NotNull(res);
        Assert.Equal("a.csv", res!.FileName);
        await repo.Received(1).GetByFileNameAsync("a.csv");
    }
}