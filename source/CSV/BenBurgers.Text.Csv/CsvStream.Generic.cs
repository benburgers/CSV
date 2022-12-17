/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Mapping;
using System.Collections.Generic;

namespace BenBurgers.Text.Csv;

/// <summary>
/// A stream that contains CSV data.
/// </summary>
/// <typeparam name="T">The type of CSV record.</typeparam>
public sealed class CsvStream<T> : CsvStream
{
    private readonly ICsvMapping<T> mapping;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvStream" />.
    /// </summary>
    /// <param name="stream">The stream that contains the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvStream(Stream stream, CsvOptions<T> options)
        : base(stream, options)
    {
        this.mapping = options.Mapping;
    }

    /// <summary>
    /// Reads a record from the stream.
    /// </summary>
    /// <returns>The record.</returns>
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown if the mapping is not supported.
    /// </exception>
    public new T? ReadLine()
    {
        var rawValues = base.ReadLine();
        if (rawValues is null)
            return default;
        return this.mapping switch
        {
            ICsvLinearMapping<T> linearMapping => linearMapping.Consumer(rawValues),
            ICsvHeaderMapping<T> headerMapping => headerMapping.Consumer(
                this
                    .ColumnNames
                    .Zip(rawValues, (cn, v) => new KeyValuePair<string, string>(cn, v))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
            _ => throw new NotSupportedException()
        };
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Use <see cref="InsertLine(long, T)" /> instead.
    /// </exception>
    public override void InsertLine(long lineNumber, IReadOnlyList<string> values) =>
        throw new InvalidOperationException();

    /// <summary>
    /// Inserts a record at the specified line number.
    /// </summary>
    /// <param name="lineNumber">The line number at which to insert the record.</param>
    /// <param name="record">The record to insert.</param>
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown if the mapping is not supported.
    /// </exception>
    public void InsertLine(long lineNumber, T record)
    {
        switch (this.mapping)
        {
            case ICsvLinearMapping<T> linearMapping:
                {
                    var rawValues = linearMapping.Producer(record);
                    base.InsertLine(lineNumber, rawValues);
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
                    base.InsertLine(lineNumber, lineValues);
                }
                break;
            default:
                throw new NotSupportedException();
        }
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Use <see cref="InsertLineAsync(long, T, CancellationToken)" /> instead.
    /// </exception>
    public override Task InsertLineAsync(long lineNumber, IReadOnlyList<string> values, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException();

    /// <summary>
    /// Inserts a record at the specified line number.
    /// </summary>
    /// <param name="lineNumber">The line number at which to insert the record.</param>
    /// <param name="record">The record to insert.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown if the mapping is not supported.
    /// </exception>
    public async Task InsertLineAsync(long lineNumber, T record, CancellationToken cancellationToken = default)
    {
        switch (this.mapping)
        {
            case ICsvLinearMapping<T> linearMapping:
                {
                    var rawValues = linearMapping.Producer(record);
                    await base.InsertLineAsync(lineNumber, rawValues, cancellationToken);
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
                    await base.InsertLineAsync(lineNumber, lineValues, cancellationToken);
                }
                break;
            default:
                throw new NotSupportedException();
        }
    }
}
