/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Exceptions;

/// <summary>
/// An exception that is thrown if a CSV query fails.
/// </summary>
public class CsvQueryException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">An optional inner exception.</param>
    internal CsvQueryException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
