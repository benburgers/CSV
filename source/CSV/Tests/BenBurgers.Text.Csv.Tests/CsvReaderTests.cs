/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Text;

namespace BenBurgers.Text.Csv.Tests;

public partial class CsvReaderTests
{
    [Theory(DisplayName = "CSV Reader - raw values")]
    [ClassData(typeof(CsvReaderTestRawValues))]
    public async Task RawTestAsync(
        string input,
        CsvOptions options,
        string[][] expected)
    {
        // Arrange
        var inputData = Encoding.UTF8.GetBytes(input);
        using var stream = new MemoryStream(inputData);
        using var reader = new CsvReader(stream, options);
        var lines = new List<IReadOnlyList<string>>();

        // Act
        while (await reader.ReadLineAsync() is { } line)
        {
            lines.Add(line);
        }

        // Assert
        Assert.NotEmpty(lines);
        Assert.Equal(expected, lines);
    }
}
