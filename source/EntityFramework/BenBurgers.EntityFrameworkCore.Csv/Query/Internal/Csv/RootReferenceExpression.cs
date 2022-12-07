/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression at the root of an entity query.
/// </summary>
internal sealed partial class RootReferenceExpression : Expression, IAccessExpression
{
    /// <summary>
    /// Initializes a new instance of <see cref="RootReferenceExpression" />.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    internal RootReferenceExpression(IEntityType entityType)
    {
        this.EntityType = entityType;
    }

    /// <summary>
    /// Gets the entity type.
    /// </summary>
    public IEntityType EntityType { get; }

    /// <inheritdoc />
    public string Name => this.EntityType.Name;

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <inheritdoc />
    public override Type Type => this.EntityType.ClrType;

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => this;
}
