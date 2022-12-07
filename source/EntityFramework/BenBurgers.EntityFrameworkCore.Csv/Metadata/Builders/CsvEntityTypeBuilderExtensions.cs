/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;

/// <summary>
/// Extension methods for <see cref="EntityTypeBuilder" /> when processing CSV data.
/// </summary>
public static class CsvEntityTypeBuilderExtensions
{
    /// <summary>
    /// Maps the entity to a CSV data source.
    /// </summary>
    /// <param name="entityTypeBuilder">The entity type builder.</param>
    /// <param name="csvDataSource">The CSV data source.</param>
    /// <returns>The entity type builder.</returns>
    public static EntityTypeBuilder ToCsvDataSource(
        this EntityTypeBuilder entityTypeBuilder,
        CsvDataSource csvDataSource)
        => entityTypeBuilder.HasAnnotation(CsvAnnotationNames.DataSource, csvDataSource);
}
