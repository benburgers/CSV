/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping for CSV records using a converter that converts raw CSV values to a record.
/// </summary>
/// <typeparam name="T">The type of the CSV record.</typeparam>
public sealed class CsvConverterMapping<T> : ICsvLinearMapping<T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvConverterMapping{T}" />.
    /// </summary>
    /// <param name="producer">A producer function that translates a record of <typeparamref name="T" /> to raw CSV values</param>
    /// <param name="consumer">A consumer function that translates raw CSV values to a record of <typeparamref name="T" />.</param>
    public CsvConverterMapping(
        Func<T, IReadOnlyList<string>> producer,
        Func<IReadOnlyList<string>, T> consumer)
    {
        this.Consumer = consumer;
        this.Producer = producer;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Always <see langword="null" /> because the converter function takes care of conversion, unbeknownst of the column names.
    /// </remarks>
    public IReadOnlySet<string>? ColumnNames { get; } = null;

    /// <summary>
    /// Gets the consumer function that translates raw CSV values to a record of <typeparamref name="T" />.
    /// </summary>
    public Func<IReadOnlyList<string>, T> Consumer { get; }

    /// <summary>
    /// Gets the producer function that translates a record of <typeparamref name="T" /> to raw CSV values.
    /// </summary>
    public Func<T, IReadOnlyList<string>> Producer { get; }
}
