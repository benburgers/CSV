/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv;

public partial class CsvStream
{
    /// <summary>
    /// Writes a line to the stream.
    /// </summary>
    /// <param name="values">A list of raw values.</param>
    public void AppendLine(IReadOnlyList<string> values)
    {
        // We need to know the new line number, so we go to the highest known line number and read until the end (the line index is lazy) and then we have an accurate count of lines.
        var line =
            this.LineIndex.Keys.Any()
                ? this.LineIndex.Keys.Max() + 1L
                : 0L;
        this.GoTo(line);
        while (this.ReadLine() is not null) { }

        // Make sure the end of the stream has new line characters (line feed (LF) and/or carriage return (CR)).
        // A CSV stream that does not have these characters at the end is valid, but the newly appended line must be written on a new line.
        var position = this.writer.EnsureEndOfStreamNewLine();

        // Write the new line at the end of the stream.
        this.writer.WriteLine(values);
        this.Line++;
        this.LineIndex[this.Line] = position;
    }

    /// <summary>
    /// Writes a line to the stream.
    /// </summary>
    /// <param name="values">A list of raw values.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    public async Task AppendLineAsync(
        IReadOnlyList<string> values,
        CancellationToken cancellationToken = default)
    {
        // We need to know the new line number, so we go to the highest known line number and read until the end (the line index is lazy) and then we have an accurate count of lines.
        var line =
            this.LineIndex.Keys.Any()
                ? this.LineIndex.Keys.Max() + 1L
                : 0L;
        await this.GoToAsync(line, cancellationToken);
        while (await this.ReadLineAsync(cancellationToken) is not null) { }

        // Make sure the end of the stream has new line characters (line feed (LF) and/or carriage return (CR)).
        // A CSV stream that does not have these characters at the end is valid, but the newly appended line must be written on a new line.
        var position = this.writer.EnsureEndOfStreamNewLine();

        // Write the new line at the end of the stream.
        await this.writer.WriteLineAsync(values, cancellationToken);
        this.Line++;
        this.LineIndex[this.Line] = position;
    }
}
