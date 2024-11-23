/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping from raw CSV values to a formalized CSV record using the order of the columns.
/// </summary>
/// <typeparam name="T">The type of CSV record.</typeparam>
public interface ICsvLinearMapping<T> : ICsvMapping<T>
{
    /// <summary>
    /// Gets the consumer function that translates raw CSV values to a record of <typeparamref name="T" />.
    /// </summary>
    Func<IReadOnlyList<string>, T> Consumer { get; }

    /// <summary>
    /// Gets the producer function that translates a record of <typeparamref name="T" /> to raw CSV values.
    /// </summary>
    Func<T, IReadOnlyList<string>> Producer { get; }
}
