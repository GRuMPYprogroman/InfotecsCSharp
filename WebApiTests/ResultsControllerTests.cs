using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.ResultsFilters;
using TimescaleService.Infrastructure.Controllers;

namespace WebApiTests;

public class ResultsControllerTests
{
    [Fact]
    public async Task GetFilteredData_Should_BadRequest_On_Invalid_DateRange()
    {
        var pipeline = Substitute.For<IResultsFilterPipeline>();
        
        pipeline
            .ExecuteAsync(Arg.Any<ResultsFilterRequest>())
            .Returns(new List<AggregatedResult> { new() { FileName = "a.csv" } });
        
        var controller = new ResultsController(pipeline);

        var req = new ResultsFilterRequest
        {
            MinimumDateFrom = new DateTime(2024, 2, 1),
            MinimumDateTo = new DateTime(2024, 1, 1),
        };

        var resp = await controller.GetFilteredData(req);

        var bad = Assert.IsType<BadRequestObjectResult>(resp.Result);
        Assert.Equal("MinimumDateFrom must be <= MinimumDateTo", bad.Value);
    }

    [Fact]
    public async Task GetFilteredData_Should_Return_Ok_With_Pipeline_Result()
    {
        var pipeline = Substitute.For<IResultsFilterPipeline>();
        
        pipeline
            .ExecuteAsync(Arg.Any<ResultsFilterRequest>())
            .Returns(new List<AggregatedResult> { new() { FileName = "a.csv" } });
        
        var controller = new ResultsController(pipeline);

        pipeline.ExecuteAsync(Arg.Any<ResultsFilterRequest>())
            .Returns(Task.FromResult(new List<AggregatedResult> { new() { FileName = "a.csv" } }));

        var resp = await controller.GetFilteredData(new ResultsFilterRequest());

        var ok = Assert.IsType<OkObjectResult>(resp.Result);
        var data = Assert.IsAssignableFrom<IReadOnlyList<AggregatedResult>>(ok.Value);
        
        Assert.Single(data);
        Assert.Equal("a.csv", data[0].FileName);
    }
}