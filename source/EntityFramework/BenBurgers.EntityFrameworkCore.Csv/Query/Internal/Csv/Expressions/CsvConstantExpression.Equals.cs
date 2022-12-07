/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

internal sealed partial class CsvConstantExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            CsvConstantExpression constantExpression => this.Equals(constantExpression),
            _ => false
        };
    }

    private bool Equals(CsvConstantExpression other)
        => base.Equals(other)
            && (this.Value?.Equals(other) ?? other.Value is null);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), this.Value);
}
