/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv.Mapping.Exceptions;

/// <summary>
/// An exception that is thrown if the type converter encounters a value that is not a string when a conversion to another type is attempted.
/// </summary>
public sealed class CsvTypeConverterValueMustBeStringException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvTypeConverterValueMustBeStringException" />.
    /// </summary>
    internal CsvTypeConverterValueMustBeStringException()
        : base(ExceptionMessages.TypeConverterValueMustBeString)
    {
    }
}
