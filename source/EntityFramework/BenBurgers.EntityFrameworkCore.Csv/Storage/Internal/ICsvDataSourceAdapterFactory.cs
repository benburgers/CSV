/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// An adapter for reading and writing from a CSV data source.
/// </summary>
internal interface ICsvDataSourceAdapterFactory
{
    /// <summary>
    /// Creates a data source adapter for the specified <paramref name="dataSource" />.
    /// </summary>
    /// <param name="dataSource">The data source.</param>
    /// <returns>The data source adapter.</returns>
    ICsvDataSourceAdapter Create(CsvDataSource dataSource);
}
