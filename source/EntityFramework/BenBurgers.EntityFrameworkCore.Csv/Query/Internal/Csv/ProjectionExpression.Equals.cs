/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

internal sealed partial class ProjectionExpression
{
    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj switch
        {
            null => false,
            _ when ReferenceEquals(this, obj) => true,
            ProjectionExpression projectionExpression => this.Equals(projectionExpression),
            _ => false
        };

    private bool Equals(ProjectionExpression other)
        => this.Expression.Equals(other.Expression);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(this.Expression);
}
