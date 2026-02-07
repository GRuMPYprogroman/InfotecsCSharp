using System.Text;
using TimescaleService.Core.Services.Parser;

namespace WebApiTests;

public class CsvParserServiceTests
{
    private MemoryStream Ms(string s) => new(Encoding.UTF8.GetBytes(s));

    [Fact]
    public void Parse_Should_Throw_When_Stream_Is_Null()
    {
        var sut = new CsvParserService();
        Assert.Throws<ArgumentNullException>(() => sut.Parse(null!, "a.csv"));
    }

    [Fact]
    public void Parse_Should_Throw_When_Stream_Is_Not_Readable()
    {
        var sut = new CsvParserService();
        using var ms = Ms("2024-01-01T00:00:00Z;1;2.0");
        ms.Close();

        Assert.Throws<ArgumentException>(() => sut.Parse(ms, "a.csv"));
    }

    [Fact]
    public void Parse_Should_Parse_Valid_Lines_And_Skip_Invalid_Ones()
    {
        var sut = new CsvParserService();

        var csv =
            "2024-01-01T00:00:00Z;10;1.5\n" +
            "badline\n" +
            "2024-01-01T00:00:00Z;xx;1.5\n" +
            "2024-01-01T00:00:00Z;10;yy\n";

        using var ms = Ms(csv);

        var res = sut.Parse(ms, "a.csv");

        Assert.Single(res);
        var row = res.First();
        Assert.Equal("a.csv", row.FileName);
        Assert.Equal(10, row.ExecutionTime);
        Assert.Equal(1.5, row.Value);
        Assert.Equal(DateTimeKind.Utc, row.Date.Kind);
    }

    [Fact]
    public void Parse_Should_Throw_CsvParsingException_When_No_Valid_Rows()
    {
        var sut = new CsvParserService();
        using var ms = Ms("bad;line\n");

        Assert.Throws<CsvParsingException>(() => sut.Parse(ms, "a.csv"));
    }
}