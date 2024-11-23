/*
 * © 2022-2024 Ben Burgers and contributors.
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
    /// Gets the set of column names.
    /// </summary>
    IReadOnlySet<string>? ColumnNames { get; }
}
