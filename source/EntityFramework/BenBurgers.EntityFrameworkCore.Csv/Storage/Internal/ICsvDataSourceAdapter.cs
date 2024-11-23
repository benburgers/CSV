/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// A CSV data source adapter.
/// </summary>
internal interface ICsvDataSourceAdapter
{
    /// <summary>
    /// Gets the data source.
    /// </summary>
    CsvDataSource DataSource { get; }

    /// <summary>
    /// Creates the data source if it does not already exist.
    /// </summary>
    /// <param name="entityType">The entity type for which to create the data source.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    Task<bool> CreateIfNotExistsAsync(IReadOnlyEntityType entityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes a new line to the data source.
    /// </summary>
    /// <param name="entry">The entry to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    Task WriteNewAsync(IUpdateEntry entry, CancellationToken cancellationToken = default);
}
