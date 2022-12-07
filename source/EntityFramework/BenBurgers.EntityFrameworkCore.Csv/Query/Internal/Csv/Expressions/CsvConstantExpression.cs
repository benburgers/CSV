/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

/// <summary>
/// A constant expression.
/// </summary>
internal sealed partial class CsvConstantExpression : CsvExpression
{
    private readonly ConstantExpression constantExpression;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvConstantExpression" />.
    /// </summary>
    /// <param name="constantExpression">The constant expression.</param>
    /// <param name="typeMapping">The type mapping.</param>
    internal CsvConstantExpression(ConstantExpression constantExpression, CoreTypeMapping? typeMapping)
        : base(constantExpression.Type, typeMapping)
    {
        this.constantExpression = constantExpression;
    }

    internal object? Value
        => this.constantExpression.Value;

    /// <summary>
    /// Applies a type mapping to the <see cref="CsvConstantExpression" />.
    /// </summary>
    /// <param name="typeMapping">The type mapping to apply.</param>
    /// <returns>The <see cref="CsvExpression" /> with type mapping <paramref name="typeMapping" /> or if <see langword="null" /> the current type mapping.</returns>
    internal CsvExpression ApplyTypeMapping(CoreTypeMapping? typeMapping)
        => new CsvConstantExpression(this.constantExpression, typeMapping ?? this.TypeMapping);

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
        => this;
}
