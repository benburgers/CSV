/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

internal sealed partial class KeyAccessExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            KeyAccessExpression expression => this.Equals(expression),
            _ => false
        };
    }

    private bool Equals(KeyAccessExpression other)
        => base.Equals(other)
            && this.Name.Equals(other.Name)
            && this.AccessExpression.Equals(other.AccessExpression);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), this.Name, this.AccessExpression);
}