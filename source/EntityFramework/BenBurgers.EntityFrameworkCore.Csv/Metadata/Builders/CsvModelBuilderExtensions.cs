/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;

/// <summary>
/// Extension methods for <see cref="ModelBuilder" />.
/// </summary>
public static class CsvModelBuilderExtensions
{
    /// <summary>
    /// Returns a value indicating whether the given data source can be set as default.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <param name="dataSource">The default data source.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns><see langword="true" /> if the given data source can be set as default.</returns>
    public static bool CanSetCsvDataSourceDefault(
        this IConventionModelBuilder modelBuilder,
        CsvDataSource? dataSource,
        bool fromDataAnnotation = false)
        => modelBuilder.CanSetAnnotation(CsvAnnotationNames.DataSource, dataSource, fromDataAnnotation);

    /// <summary>
    /// Configures the default data source that will be used if no source is explicitly configured for an entity type.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <param name="dataSource">The default data source.</param>
    /// <returns>The model builder.</returns>
    public static ModelBuilder HasCsvDataSourceDefault(
        this ModelBuilder modelBuilder,
        CsvDataSource? dataSource)
    {
        modelBuilder.Model.SetCsvDataSourceDefault(dataSource);
        return modelBuilder;
    }
}
