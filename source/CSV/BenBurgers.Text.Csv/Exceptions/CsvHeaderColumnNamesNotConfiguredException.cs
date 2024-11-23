/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if the CSV data is configured to have a header line, but the column names are not present or predefined.
/// </summary>
public sealed class CsvHeaderColumnNamesNotConfiguredException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderColumnNamesNotConfiguredException" />.
    /// </summary>
    internal CsvHeaderColumnNamesNotConfiguredException()
        : base(ExceptionMessages.HeaderColumnNamesNotConfigured)
    {
    }
}
