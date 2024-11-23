/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv;

public partial class CsvStream
{
    private bool FindLine(long lineNumber)
    {
        // Check index for the highest line number before the requested line.
        var keys = this.LineIndex.Keys;
        if (keys.Count == 0)
        {
            // Index is empty, try to find the requested line from the start of the stream.
            // If a header line is required, check if it's present and skip it.
            this.reader.Seek(0L, SeekOrigin.Begin);
            if (this.reader.Options.HasHeaderLine)
            {
                if (this.reader.ReadLine() is not { } headerLine)
                    throw new CsvHeaderLineMissingException();
                this.PositionIncrement(headerLine);
            }
        }
        else
        {
            var min = keys.Last(k => k < lineNumber);
            var minPosition = this.LineIndex[min];
            this.reader.Seek(minPosition, SeekOrigin.Begin);
            this.Line = min;
        }

        // Read until the requested line is found.
        while (this.Line < lineNumber)
        {
            // If the end of the stream is reached, the line does not exist so return false.
            if (this.ReadLine() is null)
                return false;
        }

        // Line found, but the current position is at the end of the line so backtrack to the start of the line.
        this.reader.Seek(this.LineIndex[this.Line], SeekOrigin.Begin);
        return true;
    }

    private async ValueTask<bool> FindLineAsync(long lineNumber, CancellationToken cancellationToken = default)
    {
        // Check index for the highest line number before the requested line.
        var keys = this.LineIndex.Keys;
        if (keys.Count == 0)
        {
            // Index is empty, try to find the requested line from the start of the stream.
            // If a header line is required, check if it's present and skip it.
            this.reader.Seek(0L, SeekOrigin.Begin);
            if (this.reader.Options.HasHeaderLine)
            {
                if (await this.reader.ReadLineAsync(cancellationToken) is not { } headerLine)
                    throw new CsvHeaderLineMissingException();
                this.PositionIncrement(headerLine);
            }
        }
        else
        {
            var min = keys.Last(k => k < lineNumber);
            var minPosition = this.LineIndex[min];
            this.reader.Seek(minPosition, SeekOrigin.Begin);
            this.Line = min;
        }

        // Read until the requested line is found.
        while (this.Line < lineNumber)
        {
            // If the end of the stream is reached, the line does not exist so return false.
            if (await this.ReadLineAsync(cancellationToken) is null)
                return false;
        }

        // Line found, but the current position is at the end of the line so backtrack to the start of the line.
        this.reader.Seek(this.LineIndex[this.Line], SeekOrigin.Begin);
        return true;
    }

    /// <summary>
    /// Moves the stream to the specified line.
    /// </summary>
    /// <param name="lineNumber">The line to go to.</param>
    /// <returns>A value that indicates whether the line was found.</returns>
    /// <exception cref="InvalidOperationException">A <see cref="InvalidOperationException" /> is thrown if the stream does not allow seeking to a particular position on the stream.</exception>
    /// <exception cref="CsvLineOutOfBoundsException">The specified <paramref name="lineNumber" /> is out of bounds of the CSV stream.</exception>
    /// <exception cref="CsvHeaderLineMissingException">A <see cref="CsvHeaderLineMissingException" /> is thrown if the header line is required, but missing.</exception>
    public bool GoTo(long lineNumber)
    {
        if (!this.reader.CanSeek || !this.writer.CanSeek)
            throw new InvalidOperationException();
        if (lineNumber < 0L)
            throw new CsvLineOutOfBoundsException(lineNumber);
        if (!this.LineIndex.TryGetValue(lineNumber, out long position))
            return this.FindLine(lineNumber);
        this.reader.Seek(position, SeekOrigin.Begin);
        return true;
    }

    /// <summary>
    /// Moves the stream to the specified line.
    /// </summary>
    /// <param name="line">The line to go to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An value that indicates whether the line was found.</returns>
    /// <exception cref="InvalidOperationException">A <see cref="InvalidOperationException" /> is thrown if the stream does not allow seeking to a particular position on the stream.</exception>
    /// <exception cref="CsvLineOutOfBoundsException">The specified <paramref name="line" /> is out of bounds of the CSV stream.</exception>
    /// <exception cref="CsvHeaderLineMissingException">A <see cref="CsvHeaderLineMissingException" /> is thrown if the header line is required but missing.</exception>
    public async ValueTask<bool> GoToAsync(long line, CancellationToken cancellationToken = default)
    {
        if (!this.reader.CanSeek || !this.writer.CanSeek)
            throw new InvalidOperationException();
        if (line < 0L)
            throw new CsvLineOutOfBoundsException(line);
        if (!this.LineIndex.TryGetValue(line, out long position))
            return await this.FindLineAsync(line, cancellationToken);
        this.reader.Seek(position, SeekOrigin.Begin);
        return true;
    }
}
