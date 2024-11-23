/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if a line was sought that was out of bounds of a CSV stream.
/// </summary>
public sealed class CsvLineOutOfBoundsException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvLineOutOfBoundsException" />.
    /// </summary>
    /// <param name="line">The line that was out of bounds.</param>
    internal CsvLineOutOfBoundsException(long line)
        : base(GetExceptionMessage(line))
    {
        this.Line = line;
    }

    /// <summary>
    /// Gets the line that was out of bounds.
    /// </summary>
    public long Line { get; }

    private static string GetExceptionMessage(long line)
        => string.Format(ExceptionMessages.LineOutOfBounds, line);
}
