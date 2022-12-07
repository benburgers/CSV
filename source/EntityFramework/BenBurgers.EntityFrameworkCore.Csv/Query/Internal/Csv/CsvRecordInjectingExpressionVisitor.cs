/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// Injects a CSV record into a query expression.
/// </summary>
internal sealed class CsvRecordInjectingExpressionVisitor : ExpressionVisitor
{
    private readonly Type csvRecordType;
    private int currentEntityIndex;

    internal CsvRecordInjectingExpressionVisitor(Type csvRecordType)
    {
        this.csvRecordType = csvRecordType;
    }

    /// <summary>
    /// Visits an extension expression.
    /// </summary>
    /// <param name="node">The extension expression.</param>
    /// <returns>An expression that was changed to contain the CSV record variable.</returns>
    protected override Expression VisitExtension(Expression node)
    {
        switch (node)
        {
            case EntityShaperExpression shaperExpression:
                {
                    this.currentEntityIndex++;
                    var valueBufferExpression = shaperExpression.ValueBufferExpression;
                    var csvRecordVariable = Expression.Variable(this.csvRecordType, $"csvRecord{this.currentEntityIndex}");
                    var variables = new List<ParameterExpression> { csvRecordVariable };
                    var expressions = new List<Expression>
                    {
                        Expression.Assign(csvRecordVariable, Expression.TypeAs(valueBufferExpression, this.csvRecordType)),
                        Expression.Condition(
                            Expression.Equal(csvRecordVariable, Expression.Constant(null, csvRecordVariable.Type)),
                            Expression.Constant(null, csvRecordVariable.Type),
                            shaperExpression)
                    };
                    return Expression.Block(
                        shaperExpression.Type,
                        variables,
                        expressions);
                }
        }
        return base.VisitExtension(node);
    }
}
