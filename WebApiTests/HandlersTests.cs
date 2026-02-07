using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Repositories;
using TimescaleService.Core.ResultsFilters;
using TimescaleService.Core.ResultsFilters.Handlers;

namespace WebApiTests;

public class HandlersTests
{
    [Fact]
    public async Task Pipeline_Should_Chain_Handlers_And_Return_Result()
    {
        var repo = Substitute.For<IResultsRepository>();

        var file = new FileNameHandler(repo);
        var minDate = new MinimumDateRangeHandler(repo);
        var avgValue = new AverageValueRangeHandler(repo);
        var avgExec = new AverageExecTimeRangeHandler(repo);
        var terminal = new TerminalHandler();

        var pipeline = new ResultsFilterPipeline(file, minDate, avgValue, avgExec, terminal);

        repo.GetByFileNameAsync("a.csv").Returns(new AggregatedResult()
        {
            FileName = "a.csv",
            MinimumDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            AverageValue = 100,
            AverageExecTime = 5,
        });

        var res = await pipeline.ExecuteAsync(new ResultsFilterRequest { FileName = "a.csv", AverageValueMin = 50 });

        Assert.Single(res);
        Assert.Equal("a.csv", res[0].FileName);
    }
}