/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Storage;

/// <summary>
/// A CSV data source of a file.
/// </summary>
public sealed class CsvFileSource : CsvDataSource
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvFileSource" />.
    /// </summary>
    /// <param name="path">The path to the CSV file.</param>
    /// <param name="delimiter">The column delimiter.</param>
    /// <param name="hasColumnNamesRow">Indicates whether the CSV file has a leading column names row.</param>
    public CsvFileSource(
        string path,
        char delimiter = ',',
        bool hasColumnNamesRow = false)
        : base(delimiter, hasColumnNamesRow)
    {
        this.Path = path;
    }

    /// <summary>
    /// Gets the path to the CSV file.
    /// </summary>
    public string Path { get; }
}
