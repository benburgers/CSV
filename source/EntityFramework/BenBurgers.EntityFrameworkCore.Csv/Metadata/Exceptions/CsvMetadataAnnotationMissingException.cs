/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;

/// <summary>
/// An exception that is thrown if a required annotation is missing.
/// </summary>
public sealed class CsvMetadataAnnotationMissingException : CsvMetadataException
{
    internal CsvMetadataAnnotationMissingException(string annotationName)
        : base(GetExceptionMessage(annotationName))
    {
        this.AnnotationName = annotationName;
    }

    private static string GetExceptionMessage(string annotationName)
    {
        return string.Format(ExceptionMessages.AnnotationMissing, annotationName);
    }

    /// <summary>
    /// Gets the annotation name.
    /// </summary>
    public string AnnotationName { get; }
}
