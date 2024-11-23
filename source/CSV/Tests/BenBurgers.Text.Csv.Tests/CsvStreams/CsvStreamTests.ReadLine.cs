/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Tests.CsvStreams;

public partial class CsvStreamTests
{
    public static readonly IEnumerable<object?[]> ReadLineParameters =
        new[]
        {
            new object?[]
            {
                "Sample",
                new CsvOptions(),
                new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" }
            },
            new object?[]
            {
                "Sample_header",
                new CsvOptions(HasHeaderLine: true),
                new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" }
            }
        };

    [Theory(DisplayName = "CSV Stream :: ReadLine")]
    [MemberData(nameof(ReadLineParameters))]
    public void ReadLineTest(
        string name,
        CsvOptions csvOptions,
        string[]? expected)
    {
        // Arrange
        using var stream = GetSample(name);
        using var csvStream = new CsvStream(stream, csvOptions);

        // Act
        var actual = csvStream.ReadLine();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "CSV Stream :: ReadLineAsync")]
    [MemberData(nameof(ReadLineParameters))]
    public async Task ReadLineTestAsync(
        string name,
        CsvOptions csvOptions,
        string[]? expected)
    {
        // Arrange
        using var stream = GetSample(name);
        using var csvStream = new CsvStream(stream, csvOptions);

        // Act
        var actual = await csvStream.ReadLineAsync();

        // Assert
        Assert.Equal(expected, actual);
    }
}
