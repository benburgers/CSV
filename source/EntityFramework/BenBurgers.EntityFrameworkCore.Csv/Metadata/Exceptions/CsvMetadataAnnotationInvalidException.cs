/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Exceptions;

/// <summary>
/// An exception that is thrown if a CSV metadata annotation has an invalid value.
/// </summary>
public sealed class CsvMetadataAnnotationInvalidException : CsvMetadataException
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvMetadataAnnotationInvalidException" />.
    /// </summary>
    /// <param name="annotationName">The annotation name.</param>
    internal CsvMetadataAnnotationInvalidException(string annotationName)
        : base(GetExceptionMessage(annotationName))
    {
        this.AnnotationName = annotationName;
    }

    /// <summary>
    /// Gets the annotation name.
    /// </summary>
    public string AnnotationName { get; }

    private static string GetExceptionMessage(string annotationName)
    {
        return string.Format(ExceptionMessages.AnnotationInvalid,  annotationName);
    }
}
