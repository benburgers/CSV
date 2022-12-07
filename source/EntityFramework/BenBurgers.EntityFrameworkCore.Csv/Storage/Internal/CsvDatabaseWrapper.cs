/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// Wraps an Entity Framework Database in order to process requests concerning CSV data.
/// </summary>
internal sealed class CsvDatabaseWrapper : Database
{
    private readonly ICsvDataSourceAdapterFactory dataSourceAdapterFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvDatabaseWrapper" />.
    /// </summary>
    /// <param name="dependencies">The database dependencies.</param>
    /// <param name="dataSourceAdapterFactory">The data source adapter factory.</param>
    public CsvDatabaseWrapper(
        DatabaseDependencies dependencies,
        ICsvDataSourceAdapterFactory dataSourceAdapterFactory)
        : base(dependencies)
    {
        this.dataSourceAdapterFactory = dataSourceAdapterFactory;
    }

    /// <summary>
    /// Creates the data source if it does not exist.
    /// </summary>
    /// <param name="entityType">The entity for which to create the data source.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An awaitable task.</returns>
    internal async Task<bool> CreateDataSourceIfNotExistsAsync(
        IReadOnlyEntityType entityType,
        CancellationToken cancellationToken = default)
    {
        var dataSource = entityType.GetCsvDataSource();
        if (dataSource is null)
            throw new CsvMetadataDataSourceNotFoundOrRecognizedException(entityType);
        var adapter = this.dataSourceAdapterFactory.Create(dataSource);
        return await adapter.CreateIfNotExistsAsync(entityType, cancellationToken);
    }

    /// <inheritdoc />
    public override int SaveChanges(IList<IUpdateEntry> entries)
    {
        var numChanges = 0;
        foreach (var entry in entries)
        {
            var entityType = entry.EntityType;
            var dataSource = entityType.GetCsvDataSource();
            if (dataSource is null)
                throw new CsvMetadataDataSourceNotFoundOrRecognizedException(entityType);
            var adapter = this.dataSourceAdapterFactory.Create(dataSource);
            switch (entry.EntityState)
            {
                case EntityState.Added:
                    adapter.WriteNewAsync(entry).Wait();
                    ++numChanges;
                    break;
            }
        }
        return numChanges;
    }

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = default)
    {
        var numChanges = 0;
        foreach (var entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entityType = entry.EntityType;
            var dataSource = entityType.GetCsvDataSource();
            if (dataSource is null)
                throw new CsvMetadataDataSourceNotFoundOrRecognizedException(entityType);
            var adapter = this.dataSourceAdapterFactory.Create(dataSource);
            switch (entry.EntityState)
            {
                case EntityState.Added:
                    await adapter.WriteNewAsync(entry, cancellationToken);
                    ++numChanges;
                    break;
            }
        }
        return numChanges;
    }
}
