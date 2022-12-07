/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

internal sealed partial class RootReferenceExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            RootReferenceExpression expression => this.Equals(expression),
            _ => false
        };
    }

    private bool Equals(RootReferenceExpression other)
        => this.EntityType.Equals(other.EntityType) && this.Name.Equals(other.Name);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(this.EntityType, this.Name);
}
