/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.Text.Csv.Mapping.Exceptions;

namespace BenBurgers.Text.Csv.Mapping;

/// <summary>
/// A mapping for CSV records using a header line that contains column names.
/// </summary>
/// <typeparam name="T">The type of the CSV record.</typeparam>
public class CsvHeaderMapping<T> : ICsvHeaderMapping<T>
{
    /// <summary>Initializes a new instance of <see cref="CsvHeaderMapping{T}" />.</summary>
    /// <param name="columnNames">The expected column names.</param>
    /// <param name="consumer">The consumer for creating a new record instance.</param>
    /// <param name="producer">The value getters for each column, based on their name.</param>
    public CsvHeaderMapping(
        IReadOnlySet<string> columnNames,
        Func<IReadOnlyDictionary<string, string>, T> consumer,
        IReadOnlyDictionary<string, Func<T, string?>> producer)
    {
        this.ColumnNames = columnNames;
        this.Consumer =
            new Func<IReadOnlyDictionary<string, string>, T>(
                v =>
                {
                    if (v.Count != this.ColumnNames.Count || !v.Keys.All(cn => this.ColumnNames.Contains(cn)))
                        throw new CsvHeaderMappingColumnsMismatchException(this.ColumnNames!.ToArray(), v.Keys.ToArray());
                    var values =
                        this
                            .ColumnNames
                            .ToDictionary(cn => cn, cn => v[cn]);
                    return consumer(values);
                });
        this.Producer = new Func<T, IReadOnlyDictionary<string, string>>(r => this.ColumnNames.ToDictionary(cn => cn, cn => producer[cn](r)!));
    }

    /// <inheritdoc />
    public IReadOnlySet<string>? ColumnNames { get; }

    /// <inheritdoc />
    public Func<IReadOnlyDictionary<string, string>, T> Consumer { get; }

    /// <inheritdoc />
    public Func<T, IReadOnlyDictionary<string, string>> Producer { get; }
}
