using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services.Ports;

public interface ICsvParserService
{
    IReadOnlyCollection<Timescale> Parse(Stream csv, string filename);
}