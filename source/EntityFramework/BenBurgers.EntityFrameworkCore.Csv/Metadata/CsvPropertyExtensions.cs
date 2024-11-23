/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata;

/// <summary>
/// Extension methods for properties.
/// </summary>
public static class CsvPropertyExtensions
{
    /// <summary>
    /// Gets the CSV column of the property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>The CSV column.</returns>
    public static CsvColumn GetCsvColumn(this IReadOnlyProperty property)
        => (CsvColumn)property.GetAnnotation(CsvAnnotationNames.Column).Value!;
}
