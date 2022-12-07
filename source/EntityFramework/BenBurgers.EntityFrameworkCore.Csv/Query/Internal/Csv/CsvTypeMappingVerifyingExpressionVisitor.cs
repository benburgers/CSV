/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// Visits an expression tree and verifies that all <see cref="CsvExpression" /> have a type mapping.
/// </summary>
internal sealed class CsvTypeMappingVerifyingExpressionVisitor : ExpressionVisitor
{
    /// <inheritdoc />
    protected override Expression VisitExtension(Expression node)
    {
        if (node is CsvExpression csvExpression
            && csvExpression.TypeMapping is null)
            throw new InvalidOperationException();
        return base.VisitExtension(node);
    }
}
