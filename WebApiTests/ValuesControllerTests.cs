using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Services;
using TimescaleService.Infrastructure.Controllers;

namespace WebApiTests;

public class ValuesControllerTests
{
    [Fact]
    public async Task GetByFileName_Should_Return_Ok_And_Delegate_To_Service()
    {
        var svc = Substitute.For<IValuesService>();
        var controller = new ValuesController(svc);

        var dt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        
        svc.GetLastTenByFileName("a.csv")
            .Returns(Task.FromResult<IReadOnlyCollection<Timescale>>(new[] { new Timescale("a.csv", dt, 1, 1.0) }));

        var resp = await controller.GetByFileName("a.csv");

        var ok = Assert.IsType<OkObjectResult>(resp.Result);
        var items = Assert.IsAssignableFrom<IReadOnlyCollection<Timescale>>(ok.Value);
        Assert.Single(items);
        
        await svc.Received(1).GetLastTenByFileName("a.csv");
    }
}