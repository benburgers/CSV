/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// A projection expression.
/// </summary>
internal sealed partial class ProjectionExpression : Expression
{
    /// <summary>
    /// Initializes a new instance of <see cref="ProjectionExpression" />.
    /// </summary>
    /// <param name="expression">The projection expression.</param>
    internal ProjectionExpression(Expression expression)
    {
        this.Expression = expression;
    }

    /// <summary>
    /// Gets the projection expression.
    /// </summary>
    internal Expression Expression { get; }

    /// <summary>
    /// Gets the access name.
    /// </summary>
    internal string Name => ((IAccessExpression)this.Expression).Name;

    /// <inheritdoc />
    public override Type Type => this.Expression.Type;

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <summary>
    /// Returns a new updated <see cref="ProjectionExpression" /> if the <paramref name="expression" /> is not equal to the current member.
    /// </summary>
    /// <param name="expression">The projection expression.</param>
    /// <returns>A new updated <see cref="ProjectionExpression" /> if <paramref name="expression" /> is not equal to the current member, otherwise the same instance.</returns>
    internal ProjectionExpression Update(Expression expression)
        => !this.Expression.Equals(expression)
            ? new ProjectionExpression(expression)
            : this;

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => this.Update(visitor.Visit(this.Expression));
}
