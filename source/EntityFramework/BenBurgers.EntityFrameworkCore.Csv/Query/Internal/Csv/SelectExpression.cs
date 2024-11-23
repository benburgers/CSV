/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// A "Select" expression.
/// </summary>
internal sealed class SelectExpression : Expression
{
    private IDictionary<ProjectionMember, Expression> projectionMapping = new Dictionary<ProjectionMember, Expression>();
    private readonly List<ProjectionExpression> projection = new();

    /// <summary>
    /// Initializes a new instance of <see cref="SelectExpression" />.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    internal SelectExpression(IEntityType entityType)
    {
        this.FromExpression = new RootReferenceExpression(entityType);
        this.projectionMapping[new ProjectionMember()] = new EntityProjectionExpression(entityType, this.FromExpression);
    }

    /// <summary>
    /// Gets the expression from which to select.
    /// </summary>
    public RootReferenceExpression FromExpression { get; }

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <summary>
    /// Gets the predicate.
    /// </summary>
    internal CsvExpression? Predicate { get; private set; }

    /// <summary>
    /// Gets the projection.
    /// </summary>
    internal IReadOnlyList<ProjectionExpression> Projection
        => this.projection;

    /// <summary>
    /// Applies a predicate to the <see cref="SelectExpression" />.
    /// </summary>
    /// <param name="csvExpression">The predicate expression.</param>
    internal void ApplyPredicate(CsvExpression csvExpression)
    {
        this.Predicate = this.Predicate is null
            ? csvExpression
            : new CsvBinaryExpression(
                ExpressionType.AndAlso,
                this.Predicate,
                csvExpression,
                typeof(bool),
                csvExpression.TypeMapping);
    }

    /// <summary>
    /// Gets a mapped projection in the select expression.
    /// </summary>
    /// <param name="projectionMember">The projection member.</param>
    /// <returns>The mapped projection.</returns>
    internal Expression GetMappedProjection(ProjectionMember projectionMember)
        => this.projectionMapping[projectionMember];
}
