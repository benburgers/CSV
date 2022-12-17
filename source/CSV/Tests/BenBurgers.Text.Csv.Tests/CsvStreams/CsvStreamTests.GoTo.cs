/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Tests.CsvStreams;

public partial class CsvStreamTests
{
    public record CsvLine(long LineNumber, string[] Values);

    public static readonly IEnumerable<object?[]> GoToParameters =
        new[]
        {
            new object?[]
            {
                "Sample",
                new CsvOptions(),
                new []
                {
                    new CsvLine(2L, new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }),
                    new CsvLine(1L, new[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" })
                }
            },
            new object?[]
            {
                "Sample_header",
                new CsvOptions(HasHeaderLine: true),
                new []
                {
                    new CsvLine(2L, new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }),
                    new CsvLine(1L, new[] { "9", "8", "7", "6", "5", "4", "3", "2", "1" })
                }
            }
        };

    [Theory(DisplayName = "CSV Stream :: GoTo")]
    [MemberData(nameof(GoToParameters))]
    public void GoToTest(string name, CsvOptions csvOptions, CsvLine[] expected)
    {
        // Arrange
        using var stream = GetSample(name);
        using var csvStream = new CsvStream(stream, csvOptions);

        // Act / Assert
        foreach (var (lineNumber, values) in expected)
        {
            csvStream.GoTo(lineNumber);
            var line = csvStream.ReadLine();
            Assert.Equal(values, line);
        }
    }

    [Theory(DisplayName = "CSV Stream :: GoToAsync")]
    [MemberData(nameof(GoToParameters))]
    public async Task GoToTestAsync(string name, CsvOptions csvOptions, CsvLine[] expected)
    {
        // Arrange
        using var stream = GetSample(name);
        using var csvStream = new CsvStream(stream, csvOptions);

        // Act / Assert
        foreach (var (lineNumber, values) in expected)
        {
            await csvStream.GoToAsync(lineNumber);
            var line = await csvStream.ReadLineAsync();
            Assert.Equal(values, line);
        }
    }
}
