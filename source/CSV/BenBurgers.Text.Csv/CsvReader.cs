/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;
using System.Text;

namespace BenBurgers.Text.Csv;

/// <summary>
/// Reads CSV data.
/// </summary>
public partial class CsvReader
{
    private StreamReader streamReader;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvReader" />.
    /// </summary>
    /// <param name="stream">The stream from which to read CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    /// <exception cref="CsvHeaderLineMissingException">
    /// A <see cref="CsvHeaderLineMissingException" /> is thrown if a CSV header line was expected, but not found.
    /// </exception>
    public CsvReader(Stream stream, CsvOptions options)
    {
        this.Options = options;
        this.streamReader =
            this.GetPredefinedEncoding() is { } encoding
                ? new StreamReader(stream, encoding)
                : new StreamReader(stream);
        this.Initialize();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvReader" />.
    /// </summary>
    /// <param name="streamReader">The stream reader.</param>
    /// <param name="options">The CSV configuration options.</param>
    /// <exception cref="CsvHeaderLineMissingException">
    /// A <see cref="CsvHeaderLineMissingException" /> is thrown if a CSV header line was expected, but not found.
    /// </exception>
    public CsvReader(StreamReader streamReader, CsvOptions options)
    {
        this.streamReader = streamReader;
        this.Options = options;
        this.Initialize();
    }

    /// <summary>
    /// Gets a value that indicates whether the reader can seek to a particular position on the stream.
    /// </summary>
    public bool CanSeek => this.streamReader.BaseStream.CanSeek;

    /// <summary>
    /// Gets the column names.
    /// </summary>
    public IReadOnlyList<string> ColumnNames { get; private set; } = Array.Empty<string>();

    /// <summary>
    /// Gets the encoding.
    /// </summary>
    public Encoding Encoding => this.streamReader.CurrentEncoding;

    /// <summary>
    /// Gets a value that indicates whether the CSV reader is open.
    /// </summary>
    public bool Open => !this.disposedValue;

    /// <summary>
    /// Gets the CSV configuration options.
    /// </summary>
    public CsvOptions Options { get; }

    /// <summary>
    /// Determines an optional encoding.
    /// If no encoding is specified, the reader attempts to find it in the stream.
    /// </summary>
    /// <returns>The predefined encoding.</returns>
    private Encoding? GetPredefinedEncoding() =>
        this.Options.CodePage is { } codePage
            ? Encoding.GetEncoding(codePage)
            : null;

    private void Initialize()
    {
        if (this.Options.HasHeaderLine)
        {
            var columnNamesFound = this.streamReader.ReadLine()?.Split(this.Options.Delimiter);
            var columnNamesPredefined = this.Options.ColumnNames?.ToArray();
            var columnNames = columnNamesFound ?? columnNamesPredefined;
            if (columnNames is null)
                throw new CsvHeaderLineMissingException();
            if (columnNamesPredefined is not null && columnNamesFound is not null && !columnNamesPredefined.All(cn => columnNamesFound!.Contains(cn)))
                throw new CsvHeaderColumnNamesNotConfiguredException();
            this.ColumnNames = columnNames;
        }
    }

    /// <summary>
    /// Reads a line from the CSV data.
    /// </summary>
    /// <returns>A list of raw values.</returns>
    public IReadOnlyList<string>? ReadLine()
    {
        return
            this.streamReader.ReadLine() is { } line
                ? line.Split(this.Options.Delimiter)
                : null;
    }

    /// <summary>
    /// Reads a line from the CSV data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of raw values.</returns>
    /// <exception cref="OperationCanceledException">
    /// An <see cref="OperationCanceledException" /> is thrown if the <paramref name="cancellationToken" /> was triggered.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// An <see cref="ObjectDisposedException" /> is thrown if the <paramref name="cancellationToken" /> was disposed and triggered.
    /// </exception>
    public async ValueTask<IReadOnlyList<string>?> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return
            await this.streamReader.ReadLineAsync() is { } line
                ? line.Split(this.Options.Delimiter)
                : null;
    }

    /// <summary>
    /// Seeks on the stream to the specified <paramref name="offset" /> from the specified <paramref name="origin" />.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="origin">The origin.</param>
    internal void Seek(long offset, SeekOrigin origin)
    {
        var baseStream = this.streamReader.BaseStream;
        baseStream.Seek(offset, origin);
        this.streamReader = new StreamReader(baseStream);
        // TODO verify start of line
    }
}
