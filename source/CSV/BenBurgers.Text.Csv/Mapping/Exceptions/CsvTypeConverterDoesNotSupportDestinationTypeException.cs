/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv.Mapping.Exceptions;

/// <summary>
/// An exception that is thrown if the CSV type converter does not support a destination type.
/// </summary>
public sealed class CsvTypeConverterDoesNotSupportDestinationTypeException : CsvException
{
    internal CsvTypeConverterDoesNotSupportDestinationTypeException(Type targetType)
        : base(GetExceptionMessage(targetType))
    {
        this.TargetType = targetType;
    }

    /// <summary>
    /// Gets the target type.
    /// </summary>
    public Type TargetType { get; }

    private static string GetExceptionMessage(Type targetType)
    {
        return string.Format(ExceptionMessages.TypeConverterDoesNotSupportDestinationType, targetType.FullName);
    }
}
