/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Builders;

/// <summary>
/// Extension methods for <see cref="PropertyBuilder" />.
/// </summary>
internal static class CsvPropertyBuilderExtensions
{
    /// <summary>
    /// Configures the CSV column of the property.
    /// </summary>
    /// <param name="propertyBuilder">The property builder.</param>
    /// <param name="column">The CSV column.</param>
    /// <returns>The property builder.</returns>
    public static PropertyBuilder HasCsvColumn(
        this PropertyBuilder propertyBuilder,
        CsvColumn column)
        => propertyBuilder.HasAnnotation(CsvAnnotationNames.Column, column);
}
