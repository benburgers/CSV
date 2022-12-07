/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata;

/// <summary>
/// Extension methods for entity types.
/// </summary>
public static class CsvEntityTypeExtensions
{
    /// <summary>
    /// Gets the CSV columns for the <paramref name="entitytype" />.
    /// </summary>
    /// <param name="entitytype">The entity type for which to get CSV columns.</param>
    /// <returns>The CSV columns for <paramref name="entitytype" />.</returns>
    public static IReadOnlyList<CsvColumn> GetCsvColumns(this IReadOnlyEntityType entitytype)
    {
        return
            entitytype
                .GetProperties()
                .Select(p => p.GetCsvColumn())
                .OrderBy(c => c.ColumnIndex!)
                .ToArray();
    }

    private static CsvDataSource? GetCsvDataSourceDefault(this IReadOnlyEntityType entityType)
        => entityType.FindOwnership() is not null
            ? null
            : entityType.Model.GetCsvDataSourceDefault()
                ?? new CsvFileSource($"{entityType.ShortName()}.csv");

    /// <summary>
    /// Gets the CSV data source for the entity.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The CSV data source for the entity, or <see langword="null" /> if not defined.</returns>
    /// <exception cref="CsvMetadataAnnotationMissingException">A required annotation is missing.</exception>
    /// <exception cref="CsvMetadataAnnotationInvalidException">An annotation has an invalid value.</exception>
    public static CsvDataSource? GetCsvDataSource(this IReadOnlyEntityType entityType)
        => entityType.BaseType is not null
            ? entityType.GetRootType().GetCsvDataSource()
            : entityType.FindAnnotation(CsvAnnotationNames.DataSource)?.Value as CsvDataSource
                ?? GetCsvDataSourceDefault(entityType);

    /// <summary>
    /// Sets a CSV data source for the entity.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <param name="csvDataSource">The CSV data source for the entity.</param>
    public static void SetCsvDataSource(
        this IMutableEntityType entityType,
        CsvDataSource csvDataSource)
        => entityType.SetOrRemoveAnnotation(CsvAnnotationNames.DataSource, csvDataSource);
}
