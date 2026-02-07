using Microsoft.AspNetCore.Mvc;
using TimescaleService.Core.Services;
using TimescaleService.Core.Services.Ports;

namespace TimescaleService.Infrastructure.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly IValuesService _valuesService;
    private readonly IResultsService _resultsService;
    private readonly ICsvParserService  _csvParserService;

    public ImportController(IValuesService valuesService, IResultsService resultsService, ICsvParserService csvParserService)
    {
        _valuesService = valuesService;
        _resultsService = resultsService;
        _csvParserService = csvParserService;
    }

    public async Task<IActionResult> Import([FromForm] IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty");
        
        string fileName = file.FileName;
        
        string? ext = Path.GetExtension(fileName);
        
        if (!string.Equals(ext, ".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only .csv files are supported.");
        
        await using var stream = file.OpenReadStream();

        try
        {
            var parsedData = _csvParserService.Parse(stream, fileName);

            await _resultsService.AddAsync(parsedData);
            
            var importData = await _valuesService.AddAsync(parsedData);

            return Ok("Imported" + importData.FileName);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}