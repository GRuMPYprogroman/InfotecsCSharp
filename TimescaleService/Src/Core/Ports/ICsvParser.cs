using TimescaleService.Core.Domain;

namespace TimescaleService.Core.Services.Ports;

public interface ICsvParser
{
    IReadOnlyCollection<Timescale> Parse(Stream csv, string filename);
}