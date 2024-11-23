/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Metadata;
using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

/// <summary>
/// An expression that provides access to a key.
/// </summary>
internal sealed partial class KeyAccessExpression : CsvExpression, IAccessExpression
{
    /// <summary>
    /// Initializes a new instance of <see cref="KeyAccessExpression" />.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="accessExpression">The access expression.</param>
    /// <exception cref="CsvBindingException">A <see cref="CsvBindingException" /> is thrown if the property's column name could not be determined.</exception>
    internal KeyAccessExpression(IProperty property, Expression accessExpression)
        : base(property.ClrType, property.GetTypeMapping())
    {
        this.Name = property.GetCsvColumn().ColumnName ?? throw new CsvBindingException(property);
        this.CoreProperty = property;
        this.AccessExpression = accessExpression;
    }

    /// <summary>
    /// Gets the name of the accessed column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the property.
    /// </summary>
    internal IProperty CoreProperty { get; }

    /// <summary>
    /// Gets the access expression.
    /// </summary>
    internal Expression AccessExpression { get; }

    /// <summary>
    /// If <paramref name="outerExpression" /> is not equal to <see cref="AccessExpression" />, 
    /// returns a new <see cref="KeyAccessExpression" /> with <paramref name="outerExpression" /> as its <see cref="AccessExpression" />.
    /// Otherwise, this same instance is returned.
    /// </summary>
    /// <param name="outerExpression">The new access expression.</param>
    /// <returns>The new or current <see cref="KeyAccessExpression" />.</returns>
    internal KeyAccessExpression Update(Expression outerExpression)
        => !outerExpression.Equals(this.AccessExpression)
            ? new KeyAccessExpression(this.CoreProperty, outerExpression)
            : this;

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => this.Update(visitor.Visit(this.AccessExpression));
}
