/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// A type mapping source for CSV data.
/// </summary>
internal sealed class CsvTypeMappingSource : TypeMappingSource
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvTypeMappingSource" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvTypeMappingSource(TypeMappingSourceDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override CoreTypeMapping? FindMapping(in TypeMappingInfo mappingInfo)
    {
        return FindPrimitiveMapping(mappingInfo) ?? base.FindMapping(mappingInfo);
    }

    private static CoreTypeMapping? FindPrimitiveMapping(in TypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType;
        if (
            clrType?.IsValueType == true
            || clrType == typeof(string))
        {
            return new CsvTypeMapping(clrType);
        }

        return null;
    }
}
