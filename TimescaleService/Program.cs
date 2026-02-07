using Microsoft.EntityFrameworkCore;
using TimescaleService.Core.Repositories;
using TimescaleService.Core.ResultsFilters;
using TimescaleService.Core.ResultsFilters.Handlers;
using TimescaleService.Core.Services;
using TimescaleService.Core.Services.Parser;
using TimescaleService.Core.Services.Ports;
using TimescaleService.Infrastructure.Db.EF;
using TimescaleService.Infrastructure.Db.EF.Postgres;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

var services = builder.Services;

builder.Services.AddDbContext<TimescaleContext>(options =>
    options.UseNpgsql(connection));

services.AddScoped<IResultsService, ResultsService>();
services.AddScoped<IValuesService, ValuesService>();
services.AddScoped<ICsvParserService, CsvParserService>();
services.AddScoped<IResultsRepository, PostgresResultsRepository>();
services.AddScoped<IValuesRepository, PostgresValuesRepository>();

services.AddScoped<FileNameHandler>();
services.AddScoped<MinimumDateRangeHandler>();
services.AddScoped<AverageValueRangeHandler>();
services.AddScoped<AverageExecTimeRangeHandler>();
services.AddScoped<TerminalHandler>();

services.AddScoped<IResultsFilterPipeline, ResultsFilterPipeline>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();