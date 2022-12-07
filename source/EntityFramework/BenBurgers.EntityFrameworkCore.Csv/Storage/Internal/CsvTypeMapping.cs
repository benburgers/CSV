/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

/// <summary>
/// A type mapping for CSV data.
/// </summary>
internal sealed class CsvTypeMapping : CoreTypeMapping
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvTypeMapping" />.
    /// </summary>
    /// <param name="clrType">The CLR Type.</param>
    /// <param name="comparer">The value comparer.</param>
    /// <param name="keyComparer">The key comparer.</param>
    public CsvTypeMapping(
        Type clrType,
        ValueComparer? comparer = null,
        ValueComparer? keyComparer = null)
        : base(
            new CoreTypeMappingParameters(
                clrType,
                converter: null,
                comparer,
                keyComparer))
    {
    }

    private CsvTypeMapping(CoreTypeMappingParameters parameters)
        : base(parameters)
    {
    }

    /// <inheritdoc />
    public override CoreTypeMapping Clone(ValueConverter? converter)
        => new CsvTypeMapping(this.Parameters.WithComposedConverter(converter));
}
