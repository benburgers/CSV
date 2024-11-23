/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Metadata.Internal;

/// <summary>
/// Annotation names for CSV data.
/// </summary>
internal static class CsvAnnotationNames
{
    internal const string Prefix = "Csv:";
    internal const string Column = Prefix + nameof(Column);
    internal const string DataSource = Prefix + nameof(DataSource);
}
