/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

/// <inheritdoc />
internal sealed partial class CsvShapedQueryCompilingExpressionVisitor : ShapedQueryCompilingExpressionVisitor
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvShapedQueryCompilingExpressionVisitor" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <param name="queryCompilationContext">The query compilation context.</param>
    public CsvShapedQueryCompilingExpressionVisitor(
        ShapedQueryCompilingExpressionVisitorDependencies dependencies,
        CsvQueryCompilationContext queryCompilationContext)
        : base(dependencies, queryCompilationContext)
    {
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">
    /// A <see cref="NotSupportedException" /> is thrown if an unsupported query expression was provided.
    /// </exception>
    protected override Expression VisitShapedQuery(ShapedQueryExpression shapedQueryExpression)
    {
        var csvRecordType = shapedQueryExpression.QueryExpression.Type;
        var csvRecordParameter = Expression.Parameter(csvRecordType, "csvRecord");
        var shaperBody = shapedQueryExpression.ShaperExpression;
        shaperBody = new CsvRecordInjectingExpressionVisitor(shapedQueryExpression.QueryExpression.Type).Visit(shaperBody);
        shaperBody = this.InjectEntityMaterializers(shaperBody);

        switch (shapedQueryExpression.QueryExpression)
        {
            case SelectExpression selectExpression:
                shaperBody =
                    new CsvProjectionBindingRemovingExpressionVisitor(selectExpression, csvRecordParameter)
                        .Visit(shaperBody);
                var shaperLambda =
                    Expression
                        .Lambda(
                            shaperBody,
                            QueryCompilationContext.QueryContextParameter,
                            csvRecordParameter);
                var shaper = shaperLambda.Compile();
                return Expression.New(
                    typeof(QueryingEnumerable<,>).MakeGenericType(csvRecordType, shaperLambda.ReturnType).GetConstructors()[0],
                    Expression.Convert(QueryCompilationContext.QueryContextParameter, typeof(CsvQueryContext)),
                    Expression.Constant(selectExpression),
                    Expression.Constant(shaper));
            default:
                throw new NotSupportedException(CoreStrings.UnhandledExpressionNode(shapedQueryExpression.QueryExpression));
        }
    }
}
