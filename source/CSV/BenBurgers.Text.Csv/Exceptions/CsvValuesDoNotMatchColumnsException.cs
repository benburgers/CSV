/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if the number of CSV values does not match the number of columns.
/// </summary>
public sealed class CsvValuesDoNotMatchColumnsException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvValuesDoNotMatchColumnsException" />.
    /// </summary>
    internal CsvValuesDoNotMatchColumnsException(int columnNamesCount, int valuesCount)
        : base(GetExceptionMessage(columnNamesCount, valuesCount))
    {
    }

    /// <summary>
    /// Gets the count of the predefined or predetermined columns.
    /// </summary>
    public int ColumnNamesCount { get; }

    /// <summary>
    /// Gets the count of the values.
    /// </summary>
    public int ValuesCount { get; }

    private static string GetExceptionMessage(int columnNamesCount, int valuesCount)
    {
        return string.Format(ExceptionMessages.ValuesDoNotMatchColumns, valuesCount, columnNamesCount);
    }
}
