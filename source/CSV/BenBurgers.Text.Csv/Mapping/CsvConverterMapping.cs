/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping for CSV records using a converter that converts raw CSV values to a record.
/// </summary>
/// <typeparam name="T">The type of the CSV record.</typeparam>
/// <param name="SourceConverter">The converter that converts a CSV record to CSV values.</param>
/// <param name="DestinationConverter">The converter that converts CSV values to a CSV record.</param>
public sealed record CsvConverterMapping<T>(
    Func<T, IReadOnlyList<string>> SourceConverter,
    Func<IReadOnlyList<string>, T> DestinationConverter) : ICsvMapping<T>
{
    /// <inheritdoc />
    /// <remarks>
    /// Always <see langword="null" /> because the converter function takes care of conversion, unbeknownst of the column names.
    /// </remarks>
    public IReadOnlyList<string>? ColumnNames => null;
}
