/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

/// <summary>
/// A factory for CSV expressions.
/// </summary>
internal sealed class CsvExpressionFactory : ICsvExpressionFactory
{
    private readonly ITypeMappingSource typeMappingSource;
    private readonly IModel model;
    private readonly CoreTypeMapping boolTypeMapping;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvExpressionFactory" />.
    /// </summary>
    /// <param name="typeMappingSource">The type mapping source.</param>
    /// <param name="model">The Entity Framework Core Model.</param>
    public CsvExpressionFactory(ITypeMappingSource typeMappingSource, IModel model)
    {
        this.typeMappingSource = typeMappingSource;
        this.model = model;
        this.boolTypeMapping = typeMappingSource.FindMapping(typeof(bool), model)!;
    }

    /// <inheritdoc />
    [return: NotNullIfNotNull(nameof(csvExpression))]
    public CsvExpression? ApplyDefaultTypeMapping(CsvExpression? csvExpression)
        => csvExpression is null || csvExpression.TypeMapping is not null
            ? csvExpression
            : this.ApplyTypeMapping(csvExpression, this.typeMappingSource.FindMapping(csvExpression.Type, this.model));

    /// <inheritdoc />
    [return: NotNullIfNotNull(nameof(csvExpression))]
    public CsvExpression? ApplyTypeMapping(CsvExpression? csvExpression, CoreTypeMapping? typeMapping)
    {
        if (csvExpression is null || csvExpression.TypeMapping is not null)
            return csvExpression;
        return csvExpression switch
        {
            CsvBinaryExpression binary => this.ApplyTypeMappingOnCsvBinary(binary, typeMapping),
            CsvConstantExpression constant => constant.ApplyTypeMapping(typeMapping),
            _ => csvExpression
        };
    }

    /// <inheritdoc />
    public CsvBinaryExpression MakeBinary(
        ExpressionType operatorType,
        CsvExpression left,
        CsvExpression right,
        CoreTypeMapping? typeMapping)
    {
        var returnType = left.Type;
        switch (operatorType)
        {
            case ExpressionType.Equal:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.NotEqual:
            case ExpressionType.AndAlso:
            case ExpressionType.OrElse:
                returnType = typeof(bool);
                break;
        }
        return (CsvBinaryExpression)ApplyTypeMapping(new CsvBinaryExpression(operatorType, left, right, returnType, null), typeMapping);
    }

    /// <inheritdoc />
    public SelectExpression Select(IEntityType entityType)
        => new(entityType);

    private CsvExpression ApplyTypeMappingOnCsvBinary(
        CsvBinaryExpression csvBinaryExpression,
        CoreTypeMapping? typeMapping)
    {
        var left = csvBinaryExpression.Left;
        var right = csvBinaryExpression.Right;
        Type resultType;
        CoreTypeMapping? resultTypeMapping;
        CoreTypeMapping? inferredTypeMapping;

        switch (csvBinaryExpression.OperatorType)
        {
            case ExpressionType.Equal:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.NotEqual:
                {
                    inferredTypeMapping = ExpressionExtensions.InferTypeMapping(left, right)
                        ?? (!left.Type.Equals(typeof(object))
                            ? this.typeMappingSource.FindMapping(left.Type, this.model)
                            : this.typeMappingSource.FindMapping(right.Type, this.model));
                    resultType = typeof(bool);
                    resultTypeMapping = this.boolTypeMapping;
                }
                break;
            case ExpressionType.AndAlso:
            case ExpressionType.OrElse:
                {
                    inferredTypeMapping = this.boolTypeMapping;
                    resultType = typeof(bool);
                    resultTypeMapping = this.boolTypeMapping;
                }
                break;
            case ExpressionType.Add:
            case ExpressionType.Subtract:
            case ExpressionType.Multiply:
            case ExpressionType.Divide:
            case ExpressionType.Modulo:
            case ExpressionType.LeftShift:
            case ExpressionType.RightShift:
            case ExpressionType.And:
            case ExpressionType.Or:
                {
                    inferredTypeMapping = typeMapping ?? ExpressionExtensions.InferTypeMapping(left, right);
                    resultType = inferredTypeMapping?.ClrType ?? left.Type;
                    resultTypeMapping = inferredTypeMapping;
                }
                break;
            default:
                throw new InvalidOperationException("");
        }

        return new CsvBinaryExpression(
            csvBinaryExpression.OperatorType,
            ApplyTypeMapping(left, inferredTypeMapping),
            ApplyTypeMapping(right, inferredTypeMapping),
            resultType,
            resultTypeMapping);
    }
}
