/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

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
        return ((CsvOptions<T>)this.Options).Mapping switch
        {
            ICsvLinearMapping<T> linearMapping => linearMapping.Consumer(rawValues),
            ICsvHeaderMapping<T> headerMapping =>
                headerMapping
                    .Consumer(
                        this
                            .ColumnNames
                            .Zip(rawValues, (cn, v) => new KeyValuePair<string, string>(cn, v))
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
            _ => throw new NotSupportedException()
        };
    }
}
