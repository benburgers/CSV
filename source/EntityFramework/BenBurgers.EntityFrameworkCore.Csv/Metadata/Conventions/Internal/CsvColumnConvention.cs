/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Conventions.Internal;

/// <summary>
/// A convention that initializes unassigned CSV columns.
/// </summary>
internal sealed class CsvColumnConvention : IModelFinalizingConvention
{
    /// <inheritdoc />
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            var properties = entityType.GetProperties().ToArray();
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var columnValue = property.FindAnnotation(CsvAnnotationNames.Column)?.Value switch
                {
                    CsvColumn { ColumnIndex: { } index, ColumnName: { } name } column => column,
                    CsvColumn { ColumnIndex: null, ColumnName: { } name } => new CsvColumn(i, name),
                    CsvColumn { ColumnIndex: { } index, ColumnName: null } => new CsvColumn(index, property.Name),
                    _ => new CsvColumn(i, property.Name)
                };
                property.SetAnnotation(CsvAnnotationNames.Column, columnValue);
            }
        }
    }
}
