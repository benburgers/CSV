/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

internal abstract partial class CsvExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            CsvExpression expression => this.Equals(expression),
            _ => false
        };
    }

    /// <summary>
    /// Determines whether the specified <see cref="CsvExpression" /> is equal to the current <see cref="CsvExpression" />.
    /// </summary>
    /// <param name="other">The <see cref="CsvExpression" /> to compare with the current <see cref="CsvExpression" />.</param>
    /// <returns><see langword="true" /> if the specified <see cref="CsvExpression" /> is equal to the current <see cref="CsvExpression" />; otherwise, <see langword="false" />.</returns>
    private bool Equals(CsvExpression other)
        => this.Type.Equals(other.Type) && this.TypeMapping?.Equals(other.TypeMapping) == true;

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(this.Type, this.TypeMapping);
}
