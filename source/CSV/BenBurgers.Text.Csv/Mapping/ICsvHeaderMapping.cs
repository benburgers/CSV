/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping with a predefined header.
/// </summary>
/// <typeparam name="T">The type of CSV record that is being mapped.</typeparam>
public interface ICsvHeaderMapping<T> : ICsvMapping<T>
{
    /// <summary>
    /// Gets the consumer.
    /// </summary>
    Func<IReadOnlyDictionary<string, string>, T> Consumer { get; }

    /// <summary>
    /// Gets the producer.
    /// </summary>
    Func<T, IReadOnlyDictionary<string, string>> Producer { get; }
}
