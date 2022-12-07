/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Tests;

using System.Text;
using static CsvReaderTestGenericValues;

public partial class CsvReaderTests
{
    [Theory(DisplayName = "CSV Reader - record values")]
    [ClassData(typeof(CsvReaderTestGenericValues))]
    public async Task GenericTestAsync(
        string input,
        CsvOptions<MockCsvRecord> options,
        MockCsvRecord[] expected)
    {
        // Arrange
        var inputData = Encoding.UTF8.GetBytes(input);
        using var stream = new MemoryStream(inputData);
        using var reader = new CsvReader<MockCsvRecord>(stream, options);
        var records = new List<MockCsvRecord>();

        // Act
        while (await reader.ReadLineAsync() is { } record)
        {
            records.Add(record);
        }

        // Assert
        Assert.NotEmpty(records);
        Assert.Equal(expected, records);
    }
}
