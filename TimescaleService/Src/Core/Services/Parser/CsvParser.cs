using System.Globalization;
using System.Text;
using TimescaleService.Core.Domain;
using TimescaleService.Core.Services.Ports;

namespace TimescaleService.Core.Services.Parser;

public class CsvParser : ICsvParser
{
    private const int MaxRows = 10_000;
    
    public IReadOnlyCollection<Timescale> Parse(Stream csv, string filename)
    {
        if (csv is null)
            throw new ArgumentNullException(nameof(csv));
        
        if (!csv.CanRead)
            throw new  ArgumentException("Stream cannot be read", nameof(csv));
        
        using var reader = new StreamReader(
            csv,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true,
            bufferSize: 1024,
            leaveOpen: true);
        
        var rows = new List<Timescale>();
        string? line = null;
        
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(";");
            
            if (parts.Length != 3)
                continue;
            
            var dateText = parts[0].Trim();
            var execText = parts[1].Trim();
            var valueText = parts[2].Trim();
            
            if (dateText.Length == 0 || execText.Length == 0 || valueText.Length == 0)
                continue;

            if (!DateTime.TryParse(
                    dateText,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                    out var dateUtc))
            {
                continue;
            }

            if (!int.TryParse(execText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var exec)) 
                continue;
            
            if (!double.TryParse(
                    valueText, 
                    NumberStyles.Float | NumberStyles.AllowThousands, 
                    CultureInfo.InvariantCulture, 
                    out var val)) 
                continue;

            var timescale = new Timescale(filename, dateUtc, exec, val);
            
            rows.Add(timescale);
        }
        
        if (rows.Count == 0 || rows.Count > MaxRows)
            throw new CsvParsingException("Rows quantity must be between 1 and " + MaxRows);
        
        return rows;
    }
}