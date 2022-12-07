/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Storage;

/// <summary>
/// A CSV data source.
/// </summary>
public abstract class CsvDataSource
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvDataSource" />.
    /// </summary>
    /// <param name="delimiter">The column delimiter.</param>
    /// <param name="hasColumnNamesRow">Indicates whether the CSV source data has a leading column names row.</param>
    protected CsvDataSource(char delimiter = ',', bool hasColumnNamesRow = false)
    {
        this.Delimiter = delimiter;
        this.HasHeaderLine = hasColumnNamesRow;
    }

    /// <summary>
    /// Gets the column delimiter.
    /// </summary>
    public char Delimiter { get; }

    /// <summary>
    /// Gets a value indicating whether the CSV source data has a leading column names row.
    /// </summary>
    public bool HasHeaderLine { get; }
}
