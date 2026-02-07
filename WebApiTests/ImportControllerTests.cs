using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Services;
using TimescaleService.Core.Services.Ports;
using TimescaleService.Infrastructure.Controllers;

namespace WebApiTests;

public class ImportControllerTests
{
    private Timescale Ts(string fn, DateTime dt, int exec, double val) => new(fn, dt, exec, val);

    [Fact]
    public async Task Import_Should_Return_BadRequest_When_Empty_File()
    {
        var values = Substitute.For<IValuesService>();
        var results = Substitute.For<IResultsService>();
        var parser = Substitute.For<ICsvParserService>();

        var controller = new ImportController(values, results, parser);

        var file = Substitute.For<IFormFile>();
        file.Length.Returns(0);

        var resp = await controller.Import(file);

        var bad = Assert.IsType<BadRequestObjectResult>(resp);
        Assert.Equal("File is empty", bad.Value);
    }

    [Fact]
    public async Task Import_Should_Return_BadRequest_When_NotCsv()
    {
        var values = Substitute.For<IValuesService>();
        var results = Substitute.For<IResultsService>();
        var parser = Substitute.For<ICsvParserService>();

        var controller = new ImportController(values, results, parser);

        var file = Substitute.For<IFormFile>();
        file.Length.Returns(1);
        file.FileName.Returns("a.txt");

        var resp = await controller.Import(file);

        var bad = Assert.IsType<BadRequestObjectResult>(resp);
        Assert.Equal("Only .csv files are supported.", bad.Value);
    }

    [Fact]
    public async Task Import_Should_Return_Ok_And_Call_Services_When_Valid()
    {
        var values = Substitute.For<IValuesService>();
        var results = Substitute.For<IResultsService>();
        var parser = Substitute.For<ICsvParserService>();

        var controller = new ImportController(values, results, parser);

        var ms = new MemoryStream(new byte[] { 1, 2, 3 });
        var file = Substitute.For<IFormFile>();
        
        file.Length.Returns(ms.Length);
        file.FileName.Returns("a.csv");
        file.OpenReadStream().Returns(ms);

        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var parsed = new[] { Ts("a.csv", dt, 1, 1.0) };
        
        parser.Parse(Arg.Any<Stream>(), "a.csv").Returns(parsed);

        results.AddAsync(parsed).Returns(new AggregatedResult { FileName = "a.csv" });
        values.AddAsync(parsed).Returns(parsed[0]);

        var resp = await controller.Import(file);

        var ok = Assert.IsType<OkObjectResult>(resp);
        
        Assert.Equal("Imported a.csv", ok.Value);

        await results.Received(1).AddAsync(parsed);
        await values.Received(1).AddAsync(parsed);
    }

    [Fact]
    public async Task Import_Should_Return_BadRequest_When_Parser_Throws()
    {
        var values = Substitute.For<IValuesService>();
        var results = Substitute.For<IResultsService>();
        var parser = Substitute.For<ICsvParserService>();

        var controller = new ImportController(values, results, parser);

        var ms = new MemoryStream(new byte[] { 1, 2, 3 });
        var file = Substitute.For<IFormFile>();
        
        file.Length.Returns(ms.Length);
        file.FileName.Returns("a.csv");
        file.OpenReadStream().Returns(ms);

        parser.Parse(Arg.Any<Stream>(), "a.csv").Returns(_ => throw new Exception("boom"));

        var resp = await controller.Import(file);

        var bad = Assert.IsType<BadRequestObjectResult>(resp);
        
        Assert.Equal("boom", bad.Value);
    }
}