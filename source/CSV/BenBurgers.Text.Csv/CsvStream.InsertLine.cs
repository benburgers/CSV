/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvStream
{
    /// <summary>
    /// Inserts a new line at the specified line number.
    /// </summary>
    /// <param name="lineNumber">The zero-based number of the line.</param>
    /// <param name="values">The values to insert.</param>
    /// <exception cref="IndexOutOfRangeException">
    /// An <see cref="IndexOutOfRangeException" /> is thrown if the line number is out of bounds of the CSV data.
    /// If the new values need to be appended at the end of the stream, use <see cref="AppendLine(IReadOnlyList{string})" /> instead.
    /// </exception>
    public virtual void InsertLine(long lineNumber, IReadOnlyList<string> values)
    {
        if (!this.GoTo(lineNumber))
            throw new IndexOutOfRangeException(nameof(lineNumber));

        var currentLine = values;
        long position;
        while (this.reader.ReadLine() is { } nextLine)
        {
            position = this.Position;
            this.writer.WriteLine(currentLine);
            this.PositionIncrement(currentLine);
            this.Line++;
            this.LineIndex[this.Line] = position;
            currentLine = nextLine;
        }
        position = this.Position;
        this.writer.WriteLine(currentLine);
        this.PositionIncrement(currentLine);
        this.Line++;
        this.LineIndex[this.Line] = position;
        this.GoTo(lineNumber);
    }

    /// <summary>
    /// Inserts a new line at the specified line number.
    /// </summary>
    /// <param name="lineNumber">The zero-based number of the line.</param>
    /// <param name="values">The values to insert.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// An <see cref="IndexOutOfRangeException" /> is thrown if the line number is out of bounds of the CSV data.
    /// If the new values need to be appended at the end of the stream, use <see cref="AppendLineAsync(IReadOnlyList{string}, CancellationToken)" /> instead.
    /// </exception>
    public virtual async Task InsertLineAsync(long lineNumber, IReadOnlyList<string> values, CancellationToken cancellationToken = default)
    {
        if (!await this.GoToAsync(lineNumber))
            throw new IndexOutOfRangeException(nameof(lineNumber));

        var currentLine = values;
        long position;
        while (await this.reader.ReadLineAsync(cancellationToken) is { } nextLine)
        {
            position = this.Position;
            await this.writer.WriteLineAsync(currentLine, cancellationToken);
            this.PositionIncrement(currentLine);
            this.Line++;
            this.LineIndex[this.Line] = position;
            currentLine = nextLine;
        }
        position = this.Position;
        await this.writer.WriteLineAsync(currentLine, cancellationToken);
        this.PositionIncrement(currentLine);
        this.Line++;
        this.LineIndex[this.Line] = position;
        await this.GoToAsync(lineNumber, cancellationToken);
    }
}
