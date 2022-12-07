/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Tests;

using System.Text;
using static CsvReaderTestGenericValues;

public partial class CsvWriterTests
{
    [Theory(DisplayName = "CSV Writer - record values")]
    [ClassData(typeof(CsvWriterTestGenericValues))]
    public async Task GenericTestAsync(
        CsvOptions<MockCsvRecord> options,
        MockCsvRecord[] input,
        string expected)
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new CsvWriter<MockCsvRecord>(stream, options);

        // Act
        foreach (var record in input)
        {
            await writer.WriteLineAsync(record);
        }
        await writer.FlushAsync();

        // Assert
        var data = stream.ToArray();
        var actual = Encoding.UTF8.GetString(data);
        Assert.Equal(expected, actual);
    }
}
