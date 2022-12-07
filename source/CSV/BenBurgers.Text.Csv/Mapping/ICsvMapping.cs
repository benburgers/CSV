/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping for CSV records.
/// </summary>
/// <typeparam name="T">The type of the CSV record.</typeparam>
public interface ICsvMapping<T>
{
    /// <summary>
    /// Gets the column names.
    /// </summary>
    IReadOnlyList<string>? ColumnNames { get; }
}
