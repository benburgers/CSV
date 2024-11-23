/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Exceptions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if expression translation has failed.
/// </summary>
public class CsvTranslationException : CsvQueryException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvTranslationException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="translationErrorDetails">The translation error details.</param>
    /// <param name="innerException">The optional inner exception.</param>
    internal CsvTranslationException(
        string message,
        string? translationErrorDetails,
        Exception? innerException = null)
        : base(message, innerException)
    {
        this.TranslationErrorDetails = translationErrorDetails;
    }

    /// <summary>
    /// Gets the translation error details.
    /// </summary>
    public string? TranslationErrorDetails { get; }
}
