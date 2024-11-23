/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Text;
using BenBurgers.Text.Csv.Tests.CsvWriters;

namespace BenBurgers.Text.Csv.Tests;

public partial class CsvWriterTests
{
    [Theory(DisplayName = "CSV Writer - raw values")]
    [ClassData(typeof(CsvWriterTestRawValues))]
    public async Task RawTestAsync(CsvOptions options, string[][] values, string expected)
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new CsvWriter(stream, options);

        // Act
        foreach (var valueLine in values)
        {
            await writer.WriteLineAsync(valueLine);
        }
        await writer.FlushAsync();

        // Assert
        var data = stream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }
}
