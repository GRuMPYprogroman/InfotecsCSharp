using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;
using TimescaleService.Core.Services;

namespace WebApiTests;

public class ValuesServiceTests
{
    private Timescale Ts(string fn, DateTime dt, int exec, double val) => new(fn, dt, exec, val);

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        var repo = Substitute.For<IValuesRepository>();
        var sut = new ValuesService(repo);

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.AddAsync(null!));
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Empty()
    {
        var repo = Substitute.For<IValuesRepository>();
        var sut = new ValuesService(repo);

        await Assert.ThrowsAsync<ArgumentException>(() => sut.AddAsync(Array.Empty<Timescale>()));
    }

    [Fact]
    public async Task AddAsync_Should_Add_When_No_Existing()
    {
        var repo = Substitute.For<IValuesRepository>();
        var sut = new ValuesService(repo);

        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var input = new[] { Ts("a.csv", dt, 1, 1.0) };

        repo.GetByFileName("a.csv").Returns(Task.FromResult<IReadOnlyList<Timescale>>(Array.Empty<Timescale>()));
        repo.AddAsync(input).Returns(Task.FromResult(input[0]));

        var res = await sut.AddAsync(input);

        Assert.Equal("a.csv", res.FileName);
        await repo.Received(1).AddAsync(input);
        await repo.DidNotReceive().UpdateAsync(Arg.Any<IReadOnlyCollection<Timescale>>());
    }

    [Fact]
    public async Task AddAsync_Should_Update_When_Existing()
    {
        var repo = Substitute.For<IValuesRepository>();
        var sut = new ValuesService(repo);

        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var input = new[] { Ts("a.csv", dt, 1, 1.0) };

        repo.GetByFileName("a.csv").Returns(Task.FromResult<IReadOnlyList<Timescale>>(new[] { input[0] }));
        repo.UpdateAsync(input).Returns(Task.FromResult<Timescale?>(input[0]));

        var res = await sut.AddAsync(input);

        Assert.Equal("a.csv", res.FileName);
        await repo.Received(1).UpdateAsync(input);
        await repo.DidNotReceive().AddAsync(Arg.Any<IReadOnlyCollection<Timescale>>());
    }

    [Fact]
    public async Task GetLastTenByFileName_Should_Return_Last10_SortedByDate()
    {
        var repo = Substitute.For<IValuesRepository>();
        var sut = new ValuesService(repo);

        var baseDt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var items = Enumerable.Range(0, 20)
            .Select(i => Ts("a.csv", baseDt.AddMinutes(i), i, i))
            .OrderByDescending(x => x.Date)
            .ToList();

        repo.GetByFileName("a.csv").Returns(Task.FromResult<IReadOnlyList<Timescale>>(items));

        var res = (await sut.GetLastTenByFileName("a.csv")).ToList();

        Assert.Equal(10, res.Count);
        Assert.True(res.SequenceEqual(res.OrderBy(x => x.Date)));
        Assert.Equal(baseDt.AddMinutes(10), res.First().Date);
        Assert.Equal(baseDt.AddMinutes(19), res.Last().Date);
    }
}