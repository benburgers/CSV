/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Tests.CsvReaders;

namespace BenBurgers.Text.Csv.Tests;

using System.Text;

public partial class CsvReaderTests
{
    [Theory(DisplayName = "CSV Reader - record values")]
    [ClassData(typeof(CsvReaderTestGenericValues))]
    public async Task GenericTestAsync<T>(
        string input,
        CsvOptions<T> options,
        T[] expected)
    {
        // Arrange
        var inputData = Encoding.UTF8.GetBytes(input);
        using var stream = new MemoryStream(inputData);
        using var reader = new CsvReader<T>(stream, options);
        var records = new List<T>();

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
