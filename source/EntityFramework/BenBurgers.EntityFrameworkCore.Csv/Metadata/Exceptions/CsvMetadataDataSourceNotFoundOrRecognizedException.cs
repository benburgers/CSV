/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;

/// <summary>
/// An exception that is thrown if a CSV data source was not found or recognized for a particular entity.
/// </summary>
public sealed class CsvMetadataDataSourceNotFoundOrRecognizedException : CsvMetadataException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvMetadataDataSourceNotFoundOrRecognizedException" />.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    internal CsvMetadataDataSourceNotFoundOrRecognizedException(IReadOnlyEntityType entityType)
        : base(GetExceptionMessage(entityType))
    {
        this.EntityType = entityType;
    }

    /// <summary>
    /// Gets the entity type.
    /// </summary>
    public IReadOnlyEntityType EntityType { get; }

    private static string GetExceptionMessage(IReadOnlyEntityType entityType)
    {
        return string.Format(ExceptionMessages.EntityTypeDataSourceNotFoundOrRecognized, entityType.Name);
    }
}
