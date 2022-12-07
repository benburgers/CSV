/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;
using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// Visits expressions while converting them to <see cref="CsvExpression" />.
/// </summary>
internal sealed class CsvTranslatingExpressionVisitor : ExpressionVisitor
{
    private readonly QueryCompilationContext queryCompilationContext;
    private readonly ICsvExpressionFactory csvExpressionFactory;
    private readonly CsvTypeMappingVerifyingExpressionVisitor csvTypeMappingVerifyingExpressionVisitor;

    /// <summary>
    /// Initializes a new instance of <see cref="CsvTranslatingExpressionVisitor" />.
    /// </summary>
    /// <param name="queryCompilationContext">The query compilation context.</param>
    /// <param name="csvExpressionFactory">The CSV expression factory.</param>
    internal CsvTranslatingExpressionVisitor(
        QueryCompilationContext queryCompilationContext,
        ICsvExpressionFactory csvExpressionFactory)
    {
        this.queryCompilationContext = queryCompilationContext;
        this.csvExpressionFactory = csvExpressionFactory;
        this.csvTypeMappingVerifyingExpressionVisitor = new CsvTypeMappingVerifyingExpressionVisitor();
    }

    /// <summary>
    /// Gets the translation error details, if applicable.
    /// </summary>
    internal string? TranslationErrorDetails { get; private set; }

    private void AddTranslationErrorDetails(string details)
    {
        if (this.TranslationErrorDetails is null)
            this.TranslationErrorDetails = details;
        else
            this.TranslationErrorDetails += Environment.NewLine + details;
    }

    /// <summary>
    /// Translates an <see cref="Expression" /> to a <see cref="CsvExpression" />.
    /// </summary>
    /// <param name="expression">The expression to translate.</param>
    /// <returns>The translated <see cref="CsvExpression" />.</returns>
    internal CsvExpression? Translate(Expression expression)
    {
        this.TranslationErrorDetails = null;
        var result = this.Visit(expression);
        if (result is CsvExpression translation)
        {
            translation = this.csvExpressionFactory.ApplyDefaultTypeMapping(translation);
            if (translation.TypeMapping is null)
                return null;
            this.csvTypeMappingVerifyingExpressionVisitor.Visit(translation);
            return translation;
        }
        return null;
    }

    [DebuggerStepThrough]
    private static bool TranslationFailed(
        Expression? original,
        Expression translation,
        [NotNullWhen(false)] out CsvExpression? csvTranslation)
    {
        if (original is not null && translation is not CsvExpression)
        {
            csvTranslation = null;
            return true;
        }
        csvTranslation = (CsvExpression)translation;
        return false;
    }

    private Expression? TryBindMember(Expression source, MemberIdentity member)
    {
        if (source is not EntityReferenceExpression entityReferenceExpression)
            return null;
        var result = member.MemberInfo is not null
            ? entityReferenceExpression.ParameterEntity.BindMember(member.MemberInfo, entityReferenceExpression.Type, out _)
            : entityReferenceExpression.ParameterEntity.BindMember(member.Name ?? throw new CsvBindingException(member), entityReferenceExpression.Type, out _);
        if (result is null)
        {
            AddTranslationErrorDetails(CoreStrings.QueryUnableToTranslateMember(member.Name, entityReferenceExpression.EntityType.DisplayName()));
            throw ExceptionFactory.MemberBindingFailed(member);
        }
        return result switch
        {
            EntityProjectionExpression entityProjectionExpression => new EntityReferenceExpression(entityProjectionExpression),
            _ => result
        };
    }

    /// <inheritdoc />
    protected override Expression VisitConstant(ConstantExpression node)
        => new CsvConstantExpression(node, null);

    /// <inheritdoc />
    /// <exception cref="CsvTranslationException">
    /// A <see cref="CsvTranslationException" /> is thrown if translation failed.
    /// </exception>
    protected override Expression VisitBinary(BinaryExpression node)
    {
        // ??
        if (node.NodeType == ExpressionType.Coalesce)
        {
            var ifTrue = node.Left;
            var ifFalse = node.Right;
            if (!ifTrue.Type.Equals(ifFalse.Type))
                ifFalse = Expression.Convert(ifFalse, ifTrue.Type);
            return this.Visit(
                Expression.Condition(
                    Expression.NotEqual(ifTrue, Expression.Constant(null, ifTrue.Type)),
                    ifTrue,
                    ifFalse));
        }
        var visitedLeft = this.Visit(node.Left);
        var visitedRight = this.Visit(node.Right);
        return TranslationFailed(node.Left, visitedLeft, out var csvLeft)
            || TranslationFailed(node.Right, visitedRight, out var csvRight)
                ? throw ExceptionFactory.BinaryTranslationFailed(this.TranslationErrorDetails)
                : this.csvExpressionFactory.MakeBinary(node.NodeType, csvLeft, csvRight, null);
    }

    protected override Expression VisitExtension(Expression node)
    {
        switch (node)
        {
            case EntityProjectionExpression:
            case EntityReferenceExpression:
            case CsvExpression:
                return node;

            case EntityShaperExpression entityShaperExpression:
                var result = this.Visit(entityShaperExpression.ValueBufferExpression);
                if (result is EntityProjectionExpression entityProjectionExpression)
                    return new EntityReferenceExpression(entityProjectionExpression);
                throw new NotSupportedException();

            case ProjectionBindingExpression projectionBindingExpression:
                return projectionBindingExpression.ProjectionMember is not null
                    ? ((SelectExpression)projectionBindingExpression.QueryExpression).GetMappedProjection(projectionBindingExpression.ProjectionMember)
                    : throw new NotSupportedException();

            default:
                throw new NotSupportedException();
        }
    }

    /// <inheritdoc />
    /// <exception cref="CsvTranslationException">
    /// A <see cref="CsvTranslationException" /> is thrown if translation failed.
    /// </exception>
    protected override Expression VisitMember(MemberExpression node)
    {
        var innerExpression = this.Visit(node.Expression);
        if (innerExpression is null)
            throw ExceptionFactory.MemberTranslationFailed(node.Member, this.TranslationErrorDetails);
        var member = TryBindMember(innerExpression, MemberIdentity.Create(node.Member));
        if (member is not null)
            return member;
        return TranslationFailed(node.Expression, innerExpression, out var csvInnerExpression)
                ? throw ExceptionFactory.MemberTranslationFailed(node.Member, this.TranslationErrorDetails)
                : new CsvMemberExpression(csvInnerExpression, node.Member, csvInnerExpression.Type);
    }
}
