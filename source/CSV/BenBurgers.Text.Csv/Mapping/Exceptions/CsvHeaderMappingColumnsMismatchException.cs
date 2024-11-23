/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Exceptions;

namespace BenBurgers.Text.Csv.Mapping.Exceptions;

/// <summary>
/// An exception that is thrown if more or fewer values are read on a particular line than the number of defined columns in the header of the CSV.
/// </summary>
public sealed class CsvHeaderMappingColumnsMismatchException : CsvException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvHeaderMappingColumnsMismatchException" />.
    /// </summary>
    /// <param name="expectedColumnNames">The expected columns.</param>
    /// <param name="actualColumnNames">The actual columns.</param>
    internal CsvHeaderMappingColumnsMismatchException(IReadOnlyList<string> expectedColumnNames, IReadOnlyList<string> actualColumnNames)
        : base(CreateExceptionMessage(expectedColumnNames, actualColumnNames))
    {
    }

    private static string CreateExceptionMessage(IReadOnlyList<string> expectedColumnNames, IReadOnlyList<string> actualColumnNames)
        => string.Format(ExceptionMessages.HeaderMappingColumnsMismatch, string.Join(",", expectedColumnNames), string.Join(",", actualColumnNames));
}
