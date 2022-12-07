/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;
using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using BenBurgers.EntityFrameworkCore.Csv.Storage;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata;

/// <summary>
/// Extension methods for models.
/// </summary>
public static class CsvModelExtensions
{
    /// <summary>
    /// Gets the CSV data source for the model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The CSV data source for the model.</returns>
    /// <exception cref="CsvMetadataAnnotationMissingException">A required annotation is missing.</exception>
    /// <exception cref="CsvMetadataAnnotationInvalidException">An annotation has an invalid value.</exception>
    public static CsvDataSource? GetCsvDataSourceDefault(this IReadOnlyModel model)
    {
        return model.FindAnnotation(CsvAnnotationNames.DataSource)?.Value as CsvDataSource;
    }

    /// <summary>
    /// Sets the default CSV data source.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="dataSource">The data source to set.</param>
    public static void SetCsvDataSourceDefault(
        this IMutableModel model,
        CsvDataSource? dataSource)
        => model.SetOrRemoveAnnotation(CsvAnnotationNames.DataSource, dataSource);
}
