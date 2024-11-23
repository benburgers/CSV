/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

/// <summary>
/// Represents a CSV file.
/// </summary>
public partial class CsvStream
{
    private readonly SortedDictionary<long, long> LineIndex;
    private readonly CsvReader reader;
    private readonly CsvWriter writer;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvStream" />.
    /// </summary>
    /// <param name="stream">The stream that contains the CSV data.</param>
    /// <param name="options">The CSV options.</param>
    public CsvStream(Stream stream, CsvOptions options)
    {
        this.reader = new CsvReader(stream, options);
        this.writer = new CsvWriter(stream, options);
        this.Line = -1L;
        this.LineIndex = new();
        this.Position = 0L;
    }

    /// <summary>
    /// Gets the column names.
    /// </summary>
    public IReadOnlyList<string> ColumnNames => this.reader.ColumnNames;

    /// <summary>
    /// Gets the current line in the CSV stream.
    /// </summary>
    public long Line { get; private set; }

    /// <summary>
    /// Gets a value that indicates whether the stream is open.
    /// </summary>
    public bool Open => !this.disposedValue;

    /// <summary>
    /// Gets the CSV configuration options.
    /// </summary>
    public CsvOptions Options => this.reader.Options;

    private long Position { get; set; }

    /// <summary>
    /// Flushes the buffer.
    /// </summary>
    public void Flush() => this.writer.Flush();

    /// <summary>
    /// Flushes the buffer.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    public async Task FlushAsync(CancellationToken cancellationToken = default) => await this.writer.FlushAsync(cancellationToken);
}
