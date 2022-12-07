/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression that is a reference to an entity.
/// </summary>
internal sealed class EntityReferenceExpression : Expression
{
    /// <summary>
    /// Initializes a new instance of <see cref="EntityReferenceExpression" />.
    /// </summary>
    /// <param name="parameter">The entity projection parameter.</param>
    internal EntityReferenceExpression(EntityProjectionExpression parameter)
    {
        this.ParameterEntity = parameter;
        this.EntityType = parameter.EntityType;
        this.Type = this.EntityType.ClrType;
    }

    private EntityReferenceExpression(EntityProjectionExpression parameter, Type type)
    {
        this.ParameterEntity = parameter;
        this.EntityType = parameter.EntityType;
        this.Type = type;
    }

    /// <summary>
    /// Gets the entity projection parameter.
    /// </summary>
    internal EntityProjectionExpression ParameterEntity { get; }

    /// <summary>
    /// Gets the entity type.
    /// </summary>
    internal IEntityType EntityType { get; }

    /// <inheritdoc />
    public override Type Type { get; }

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <summary>
    /// Converts and constructs a new entity reference expression to the <paramref name="type" />.
    /// </summary>
    /// <param name="type">The type to convert to.</param>
    /// <returns>The converted expression.</returns>
    internal Expression Convert(Type type)
        => type == typeof(object)
                || type.IsAssignableFrom(this.Type)
            ? this
            : new EntityReferenceExpression(this.ParameterEntity, this.Type);
}
