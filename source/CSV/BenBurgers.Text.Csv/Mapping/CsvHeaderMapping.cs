/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping for CSV records using a header line that contains column names.
/// </summary>
/// <typeparam name="T">The type of the CSV record.</typeparam>
public class CsvHeaderMapping<T> : ICsvMapping<T>
{
    /// <summary>Initializes a new instance of <see cref="CsvHeaderMapping{T}" />.</summary>
    /// <param name="factory">The factory for creating a new record instance. Will only be called after all raw values have been passed to the value setters.</param>
    /// <param name="valueGetters">The value getters for each column, based on their name.</param>
    public CsvHeaderMapping(
        Func<IReadOnlyDictionary<string, string>, T> factory,
        IReadOnlyDictionary<string, Func<T, string?>> valueGetters)
    {
        this.ColumnNames = valueGetters.Keys.ToArray();
        this.Factory = factory;
        this.ValueGetters = valueGetters;
    }

    /// <inheritdoc />
    public IReadOnlyList<string>? ColumnNames { get; }

    /// <summary>
    /// Gets the factory for creating a new record instance. Will only be called after all raw values have been passed to the value setters.
    /// </summary>
    public Func<IReadOnlyDictionary<string, string>, T> Factory { get; }

    /// <summary>
    /// Gets the value getters for each column, based on their name.
    /// </summary>
    public IReadOnlyDictionary<string, Func<T, string?>> ValueGetters { get; }
}
