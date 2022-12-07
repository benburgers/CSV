/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if a CSV header line was expected, but wasn't found.
/// </summary>
public sealed class CsvHeaderLineMissingException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderLineMissingException" />.
    /// </summary>
    internal CsvHeaderLineMissingException()
        : base(ExceptionMessages.HeaderLineMissing)
    {
    }
}