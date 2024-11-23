/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;
using BenBurgers.Text.Csv.Mapping;

namespace BenBurgers.Text.Csv;

/// <inheritdoc />
/// <typeparam name="T">The type of the CSV record.</typeparam>
public sealed class CsvWriter<T> : CsvWriter
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvWriter{T}" />.
    /// </summary>
    /// <param name="stream">The stream to which to write the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvWriter(Stream stream, CsvOptions<T> options)
        : base(stream, options)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvWriter{T}" />.
    /// </summary>
    /// <param name="streamWriter">The stream writer with which to write the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvWriter(StreamWriter streamWriter, CsvOptions<T> options)
        : base(streamWriter, options)
    {
    }

    /// <summary>
    /// Writes a line to the CSV data while converting the CSV record to raw values.
    /// </summary>
    /// <param name="record">The CSV record to write to the CSV data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="OperationCanceledException">
    /// An <see cref="OperationCanceledException" /> is thrown if <paramref name="cancellationToken" /> is triggered.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// An <see cref="ObjectDisposedException" /> is thrown if <paramref name="cancellationToken" /> has been disposed and is triggered.
    /// </exception>
    /// <exception cref="CsvValuesDoNotMatchColumnsException">
    /// A <see cref="CsvValuesDoNotMatchColumnsException" /> is thrown if the number of values does not match the number of predefined or predetermined columns.
    /// </exception>
    public async Task WriteLineAsync(
        T record,
        CancellationToken cancellationToken = default)
    {
        var mapping = ((CsvOptions<T>)this.Options).Mapping;
        switch (mapping)
        {
            case ICsvLinearMapping<T> linearMapping:
                {
                    var rawValues = linearMapping.Producer(record);
                    await base.WriteLineAsync(rawValues, cancellationToken);
                }
                break;
            case ICsvHeaderMapping<T> headerMapping:
                {
                    var rawValues = headerMapping.Producer(record);
                    var lineValues =
                        this
                            .ColumnNames
                            .Select(cn => rawValues[cn])
                            .ToArray();
                    await base.WriteLineAsync(lineValues, cancellationToken);
                }
                break;
            default:
                throw new NotSupportedException();
        }
    }
}
