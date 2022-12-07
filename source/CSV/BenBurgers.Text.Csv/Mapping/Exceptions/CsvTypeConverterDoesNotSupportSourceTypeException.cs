/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv.Mapping.Exceptions;

/// <summary>
/// An exception that is thrown if the CSV type converter does not support a source type.
/// </summary>
public sealed class CsvTypeConverterDoesNotSupportSourceTypeException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvTypeConverterDoesNotSupportSourceTypeException" />.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    internal CsvTypeConverterDoesNotSupportSourceTypeException(Type sourceType)
        : base(GetExceptionMessage(sourceType))
    {
        this.SourceType = sourceType;
    }

    /// <summary>
    /// Gets the source type.
    /// </summary>
    public Type SourceType { get; }

    private static string GetExceptionMessage(Type sourceType)
    {
        return string.Format(ExceptionMessages.TypeConverterDoesNotSupportSourceType, sourceType.FullName);
    }
}
