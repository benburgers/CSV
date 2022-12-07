/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// Creates CSV data sources.
/// </summary>
internal sealed class CsvDatabaseCreator : IDatabaseCreator
{
    private readonly IDesignTimeModel designTimeModel;
    private readonly IUpdateAdapterFactory updateAdapterFactory;
    private readonly IDatabase database;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvDatabaseCreator" />.
    /// </summary>
    /// <param name="designTimeModel">The design time model.</param>
    /// <param name="updateAdapterFactory">The update adapter factory.</param>
    /// <param name="database">The database.</param>
    public CsvDatabaseCreator(
        IDesignTimeModel designTimeModel,
        IUpdateAdapterFactory updateAdapterFactory,
        IDatabase database)
    {
        this.designTimeModel = designTimeModel;
        this.updateAdapterFactory = updateAdapterFactory;
        this.database = database;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown.
    /// </exception>
    public bool CanConnect()
        => throw new NotSupportedException();

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown.
    /// </exception>
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        => throw new NotSupportedException();

    /// <inheritdoc />
    public bool EnsureCreated()
    {
        var entitiesCreated = new List<IEntityType>();

        var model = this.designTimeModel.Model;
        var entityTypes = model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            if (((CsvDatabaseWrapper)this.database).CreateDataSourceIfNotExistsAsync(entityType).GetAwaiter().GetResult())
                entitiesCreated.Add(entityType);
        }

        this.Seed(entitiesCreated);

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        var entitiesCreated = new List<IEntityType>();

        var model = this.designTimeModel.Model;
        var entityTypes = model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await ((CsvDatabaseWrapper)this.database).CreateDataSourceIfNotExistsAsync(entityType, cancellationToken))
                entitiesCreated.Add(entityType);
        }

        await this.SeedAsync(entitiesCreated, cancellationToken);

        return true;
    }

    /// <inheritdoc />
    public bool EnsureDeleted()
    {
        var model = this.designTimeModel.Model;
        var entityTypes = model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            var dataSource = entityType.GetCsvDataSource();
            switch (dataSource)
            {
                case CsvFileSource { Path: { } filePath }:
                    File.Delete(filePath);
                    break;
                default:
                    return false;
            }
        }
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default)
    {
        var model = this.designTimeModel.Model;
        var entityTypes = model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var dataSource = entityType.GetCsvDataSource();
            switch (dataSource)
            {
                case CsvFileSource { Path: { } filePath }:
                    File.Delete(filePath);
                    break;
                default:
                    return await Task.FromResult(false);
            }
        }
        return await Task.FromResult(true);
    }

    private void Seed(IReadOnlyList<IEntityType> entitiesCreated)
    {
        var updateAdapter = AddSeedData(entitiesCreated);
        this.database.SaveChanges(updateAdapter.GetEntriesToSave());
    }

    private Task SeedAsync(IReadOnlyList<IEntityType> entitiesCreated, CancellationToken cancellationToken = default)
    {
        var updateAdapter = AddSeedData(entitiesCreated);
        return this.database.SaveChangesAsync(updateAdapter.GetEntriesToSave(), cancellationToken);
    }

    private IUpdateAdapter AddSeedData(IReadOnlyList<IEntityType> entitiesCreated)
    {
        var updateAdapter = this.updateAdapterFactory.CreateStandalone();
        foreach (var entityType in this.designTimeModel.Model.GetEntityTypes())
        {
            if (!entitiesCreated.Any(e => e.Name == entityType.Name))
                continue;
            foreach (var targetSeed in entityType.GetSeedData())
            {
                updateAdapter.Model.FindEntityType(entityType.Name);
                var entry = updateAdapter.CreateEntry(targetSeed, entityType);
                entry.EntityState = EntityState.Added;
            }
        }

        return updateAdapter;
    }
}
