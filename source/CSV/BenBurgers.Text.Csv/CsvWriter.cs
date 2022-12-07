/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;
using System.Text;

namespace BenBurgers.Text.Csv;

/// <summary>
/// Writes CSV data.
/// </summary>
public partial class CsvWriter : IDisposable, IAsyncDisposable
{
    private readonly StreamWriter streamWriter;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvWriter" />.
    /// </summary>
    /// <param name="stream">The stream to which to write the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvWriter(Stream stream, CsvOptions options)
    {
        this.Options = options;
        this.ColumnNames = options.ColumnNames ?? new List<string>();
        this.streamWriter =
            this.GetEncoding() is { } encoding
                ? new StreamWriter(stream, encoding)
                : new StreamWriter(stream);
        this.Initialize();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvWriter" />.
    /// </summary>
    /// <param name="streamWriter">The stream writer that writes the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    public CsvWriter(StreamWriter streamWriter, CsvOptions options)
    {
        this.streamWriter = streamWriter;
        this.Options = options;
        this.ColumnNames = options.ColumnNames ?? new List<string>();
        this.Initialize();
    }

    /// <summary>
    /// Gets the column names.
    /// </summary>
    public IReadOnlyList<string> ColumnNames { get; private set; }

    /// <summary>
    /// Gets the configuration options.
    /// </summary>
    public CsvOptions Options { get; }

    private Encoding? GetEncoding()
    {
        return
            this.Options.CodePage is { } codePage
                ? Encoding.GetEncoding(codePage)
                : null;
    }

    private void Initialize()
    {
        if (this.Options.HasHeaderLine)
        {
            this.streamWriter.WriteLine(string.Join(this.Options.Delimiter, this.ColumnNames));
            this.streamWriter.Flush();
        }
    }

    /// <summary>
    /// Writes a line to the CSV data.
    /// </summary>
    /// <param name="values">The CSV values to write.</param>
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
        IReadOnlyList<string> values,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (this.Options.HasHeaderLine && this.ColumnNames is { } columnNames && columnNames.Count != values.Count)
            throw new CsvValuesDoNotMatchColumnsException(columnNames.Count, values.Count);
        await this.streamWriter.WriteLineAsync(string.Join(this.Options.Delimiter, values));
    }

    /// <summary>
    /// Flushes the buffer to the data stream.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="OperationCanceledException">
    /// An <see cref="OperationCanceledException" /> is thrown if <paramref name="cancellationToken" /> is triggered.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// An <see cref="ObjectDisposedException" /> is thrown if <paramref name="cancellationToken" /> has been disposed and is triggered.
    /// </exception>
    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await this.streamWriter.FlushAsync();
    }
}
