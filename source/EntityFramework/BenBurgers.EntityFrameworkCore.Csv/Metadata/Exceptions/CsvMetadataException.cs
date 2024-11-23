/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Exceptions;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;

/// <summary>
/// An exception that is thrown if an error occurs during the processing of CSV metadata.
/// </summary>
public class CsvMetadataException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvMetadataException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException"></param>
    internal CsvMetadataException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
