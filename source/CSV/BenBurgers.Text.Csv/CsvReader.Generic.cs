/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;
using BenBurgers.Text.Csv.Mapping;

namespace BenBurgers.Text.Csv;

/// <inheritdoc />
/// <typeparam name="T">The type of the CSV record.</typeparam>
public sealed partial class CsvReader<T> : CsvReader
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvReader{T}" />.
    /// </summary>
    /// <param name="stream">The stream from which to read the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvReader(Stream stream, CsvOptions<T> options)
        : base(stream, options)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvReader{T}" />.
    /// </summary>
    /// <param name="streamReader">The stream reader with which to read the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvReader(StreamReader streamReader, CsvOptions<T> options)
        : base(streamReader, options)
    {
    }

    private T CreateRecord(IReadOnlyList<string> rawValues)
    {
        return (((CsvOptions<T>)this.Options).Mapping) switch
        {
            CsvConverterMapping<T> { DestinationConverter: { } converter } => converter(rawValues),
            CsvHeaderMapping<T> headerMapping => CreateRecordWithHeader(headerMapping, rawValues),
            _ => throw new CsvException("")
        };
    }

    private T CreateRecordWithHeader(CsvHeaderMapping<T> headerMapping, IReadOnlyList<string> rawValues)
    {
        var values = new Dictionary<string, string>();
        for (var i = 0; i < this.ColumnNames.Count; ++i)
        {
            var columnName = this.ColumnNames[i];
            values[columnName] = rawValues[i];
        }
        return headerMapping.Factory(values);
    }

    /// <summary>
    /// Reads a line from the CSV data while converting the raw values to a CSV record.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A CSV record, if available, otherwise <see langword="default" />.</returns>
    public async new ValueTask<T?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        var rawValues = await base.ReadLineAsync(cancellationToken);
        if (rawValues is null)
            return default;
        return CreateRecord(rawValues);
    }
}
