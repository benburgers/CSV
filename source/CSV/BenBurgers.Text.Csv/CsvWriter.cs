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
public partial class CsvWriter
{
    private StreamWriter streamWriter;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvWriter" />.
    /// </summary>
    /// <param name="stream">The stream to which to write the CSV data.</param>
    /// <param name="options">The CSV configuration options.</param>
    /// <exception cref="CsvHeaderDoesNotHaveExpectedColumnNamesException">
    /// A <see cref="CsvHeaderDoesNotHaveExpectedColumnNamesException" /> is thrown if a header line is required, is present, but does not contain the expected column names.
    /// </exception>
    /// <exception cref="CsvHeaderColumnNamesNotConfiguredException">
    /// A <see cref="CsvHeaderColumnNamesNotConfiguredException" /> is thrown if a header line is required, is not present, but no column names were predefined.
    /// </exception>
    public CsvWriter(Stream stream, CsvOptions options)
    {
        this.ColumnNames = options.ColumnNames?.ToArray() ?? Array.Empty<string>();
        this.Options = options;
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
    /// <exception cref="CsvHeaderDoesNotHaveExpectedColumnNamesException">
    /// A <see cref="CsvHeaderDoesNotHaveExpectedColumnNamesException" /> is thrown if a header line is required, is present, but does not contain the expected column names.
    /// </exception>
    /// <exception cref="CsvHeaderColumnNamesNotConfiguredException">
    /// A <see cref="CsvHeaderColumnNamesNotConfiguredException" /> is thrown if a header line is required, is not present, but no column names were predefined.
    /// </exception>
    public CsvWriter(StreamWriter streamWriter, CsvOptions options)
    {
        this.ColumnNames = options.ColumnNames?.ToArray() ?? Array.Empty<string>();
        this.Options = options;
        this.streamWriter = streamWriter;
        this.Initialize();
    }

    /// <summary>
    /// Gets a value that indicates whether the writer can seek to a particular position on the stream.
    /// </summary>
    public bool CanSeek => this.streamWriter.BaseStream.CanSeek;

    /// <summary>
    /// Gets the column names.
    /// </summary>
    public IReadOnlyList<string> ColumnNames { get; private set; }

    /// <summary>
    /// Gets the encoding.
    /// </summary>
    public Encoding Encoding => this.streamWriter.Encoding;

    /// <summary>
    /// Gets the new line characters used by the <see cref="StreamWriter" />.
    /// </summary>
    internal string NewLine => this.streamWriter.NewLine;

    /// <summary>
    /// Gets a value that indicates whether the CSV writer is open.
    /// </summary>
    public bool Open => !this.disposedValue;

    /// <summary>
    /// Gets the configuration options.
    /// </summary>
    public CsvOptions Options { get; }

    /// <summary>
    /// Determines an optional encoding.
    /// If no encoding is specified, the writer attempts to find it in the stream.
    /// </summary>
    /// <returns>The predefined encoding.</returns>
    private Encoding? GetEncoding() =>
        this.Options.CodePage is { } codePage
            ? Encoding.GetEncoding(codePage)
            : null;

    private void Initialize()
    {
        if (this.Options.HasHeaderLine)
        {
            var baseStream = this.streamWriter.BaseStream;
            var positionCurrent = baseStream.Position;
            if (baseStream.CanSeek)
                baseStream.Seek(0L, SeekOrigin.Begin);
            using var streamReader = new StreamReader(baseStream, leaveOpen: true);
            var headerLine = streamReader.ReadLine();
            if (headerLine is not null)
            {
                var columnNames = headerLine.Split(this.Options.Delimiter);
                if (this.Options.ColumnNames is { } expectedColumnNames && !columnNames.Equals(expectedColumnNames))
                    throw new CsvHeaderDoesNotHaveExpectedColumnNamesException(expectedColumnNames.ToArray(), columnNames);
                this.ColumnNames = columnNames;
                return;
            }
            if (this.ColumnNames is not { } definedColumnNames)
                throw new CsvHeaderColumnNamesNotConfiguredException();
            this.ColumnNames = definedColumnNames;
            this.streamWriter.WriteLine(string.Join(this.Options.Delimiter, this.ColumnNames));
            this.streamWriter.Flush();
            if (baseStream.CanSeek && positionCurrent != 0L)
                baseStream.Seek(positionCurrent, SeekOrigin.Begin);
        }
    }

    /// <summary>
    /// Closes the CSV writer.
    /// </summary>
    public void Close()
    {
        this.streamWriter.Close();
    }
}
