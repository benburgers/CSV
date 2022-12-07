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
public partial class CsvReader : IDisposable, IAsyncDisposable
{
    private readonly StreamReader streamReader;

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
            this.GetEncoding() is { } encoding
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
    /// Gets the column names.
    /// </summary>
    public IReadOnlyList<string> ColumnNames { get; private set; } = new List<string>();

    /// <summary>
    /// Gets the CSV configuration options.
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
            if (this.streamReader.ReadLine() is not { } headerLine)
                throw new CsvHeaderLineMissingException();
            this.ColumnNames = headerLine.Split(this.Options.Delimiter);
        }
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
}
