/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression visitor that removes projection bindings.
/// </summary>
internal sealed class CsvProjectionBindingRemovingExpressionVisitor : ExpressionVisitor
{
    private readonly ParameterExpression csvRecordParameter;
    private readonly IDictionary<ParameterExpression, Expression> materializationContextBindings
        = new Dictionary<ParameterExpression, Expression>();
    private readonly IDictionary<Expression, ParameterExpression> projectionBindings
        = new Dictionary<Expression, ParameterExpression>();
    private readonly SelectExpression selectExpression;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvProjectionBindingRemovingExpressionVisitor" />.
    /// </summary>
    /// <param name="selectExpression">The select expression.</param>
    /// <param name="csvRecordParameter">The CSV record parameter.</param>
    internal CsvProjectionBindingRemovingExpressionVisitor(
        SelectExpression selectExpression,
        ParameterExpression csvRecordParameter)
    {
        this.selectExpression = selectExpression;
        this.csvRecordParameter = csvRecordParameter;
    }

    private static Expression CreateCsvRecordMemberExpression(
        Expression csvRecordExpression,
        IProperty property,
        Type type)
        => Expression.MakeMemberAccess(csvRecordExpression, ((MemberInfo?)property.FieldInfo ?? property.PropertyInfo)!);

    private Expression CreateGetValueExpression(
        Expression csvRecordExpression,
        Type type,
        CoreTypeMapping? typeMapping = null)
    {
        var innerExpression = csvRecordExpression;
        if (this.projectionBindings.TryGetValue(csvRecordExpression, out var innerVariable))
            innerExpression = innerVariable;
        else if (csvRecordExpression is RootReferenceExpression rootReferenceExpression)
            innerExpression = CreateGetValueExpression(this.csvRecordParameter, csvRecordParameter.Type);

        var csvRecordMemberExpression = CreateCsvRecordMemberExpression(innerExpression, property, );
        Expression valueExpression;
        var converter = typeMapping?.Converter;
        if (converter is not null)
        {
            var csvRecordMemberParameter = Expression.Parameter(csvRecordMemberExpression.Type);
            var body =
                ReplacingExpressionVisitor
                    .Replace(
                        converter.ConvertFromProviderExpression.Parameters.Single(),
                        Expression.Call(
                            csvRecordMemberParameter,
                            csvRecordMemberExpression));
            if (!body.Type.Equals(type))
                body = Expression.Convert(body, type);

            Expression replaceExpression;
            if (converter.ConvertsNulls)
            {
                replaceExpression =
                    ReplacingExpressionVisitor.Replace(
                        converter.ConvertFromProviderExpression.Parameters.Single(),
                        Expression.Default(converter.ProviderClrType),
                        converter.ConvertFromProviderExpression.Body);
                if (!replaceExpression.Type.Equals(type))
                    replaceExpression = Expression.Convert(replaceExpression, type);
            }
            else
                replaceExpression = Expression.Default(type);

            body =
                Expression.Condition(
                    Expression.OrElse(
                        Expression.Equal(csvRecordMemberParameter, Expression.Default(csvRecordMemberParameter.Type)),
                        Expression.Equal(
                            Expression.MakeMemberAccess(csvRecordMemberParameter, ),
                            Expression.Constant(null))),
                    replaceExpression,
                    body);

            valueExpression = Expression.Invoke(Expression.Lambda(body, csvRecordMemberParameter), csvRecordMemberExpression);
        }
        else
        {
            valueExpression = 
        }
    }

    private ProjectionExpression GetProjection(ProjectionBindingExpression projectionBindingExpression)
        => this.selectExpression.Projection[this.GetProjectionIndex(projectionBindingExpression)];

    private int GetProjectionIndex(ProjectionBindingExpression projectionBindingExpression)
        => projectionBindingExpression.ProjectionMember is not null
            ? this.selectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember).GetConstantValue<int>()
            : (projectionBindingExpression.Index ?? throw new InvalidOperationException(CoreStrings.TranslationFailed(projectionBindingExpression.Print())));

    /// <inheritdoc />
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType == ExpressionType.Assign)
        {
            if (node.Left is ParameterExpression parameterExpression)
            {
                if (parameterExpression.Type == typeof(MaterializationContext))
                {
                    var newExpression = (NewExpression)node.Right;
                    EntityProjectionExpression entityProjectionExpression;
                    if (newExpression.Arguments[0] is ProjectionBindingExpression projectionBindingExpression)
                    {
                        var projection = this.GetProjection(projectionBindingExpression);
                        entityProjectionExpression = (EntityProjectionExpression)projection.Expression;
                    } else
                    {
                        var projection = ((UnaryExpression)((UnaryExpression)newExpression.Arguments[0]).Operand).Operand;
                        entityProjectionExpression = (EntityProjectionExpression)projection;
                    }
                    this.materializationContextBindings[parameterExpression] = entityProjectionExpression.AccessExpression;
                    var updatedExpression =
                        Expression.New(
                            newExpression.Constructor!,
                            Expression.Constant(ValueBuffer.Empty),
                            newExpression.Arguments[1]);
                    return Expression.MakeBinary(ExpressionType.Assign, node.Left, updatedExpression);
                }
            }

            if (node.Left is MemberExpression { Member: FieldInfo { IsInitOnly: true } } memberExpression)
                return memberExpression.Assign(this.Visit(node.Right));
        }
        return base.VisitBinary(node);
    }

    protected override Expression VisitExtension(Expression node)
    {
        switch (node)
        {
            case ProjectionBindingExpression projectionBindingExpression:
                {
                    var projection = this.GetProjection(projectionBindingExpression);
                    return this.CreateGetValueExpression(this.csvRecordParameter, projection.Name, (projection.Expression as CsvExpression)?.TypeMapping);
                }
        }
        return base.VisitExtension(node);
    }
}
