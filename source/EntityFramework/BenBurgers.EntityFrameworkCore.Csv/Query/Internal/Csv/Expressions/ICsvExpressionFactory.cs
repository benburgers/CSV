/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;

internal interface ICsvExpressionFactory
{
    /// <summary>
    /// Apply default type mapping to <paramref name="csvExpression" />.
    /// </summary>
    /// <param name="csvExpression">The CSV expression.</param>
    /// <returns>The CSV expression with default type mapping applied, or <see langword="null" /> if <paramref name="csvExpression" /> is <see langword="null" />.</returns>
    [return: NotNullIfNotNull(nameof(csvExpression))]
    CsvExpression? ApplyDefaultTypeMapping(CsvExpression? csvExpression);

    /// <summary>
    /// Apply type mapping to <paramref name="csvExpression" />.
    /// </summary>
    /// <param name="csvExpression">The CSV expression.</param>
    /// <param name="typeMapping">The type mapping to apply.</param>
    /// <returns>The CSV expression with type mapping applied, or <see langword="null" /> if <paramref name="csvExpression" /> is <see langword="null" />.</returns>
    [return: NotNullIfNotNull(nameof(csvExpression))]
    CsvExpression? ApplyTypeMapping(CsvExpression? csvExpression, CoreTypeMapping? typeMapping);

    /// <summary>
    /// Makes a CSV binary expression.
    /// </summary>
    /// <param name="operatorType">The type of the operator.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="typeMapping">The type mapping.</param>
    /// <returns>The CSV binary expression.</returns>
    CsvBinaryExpression MakeBinary(
        ExpressionType operatorType,
        CsvExpression left,
        CsvExpression right,
        CoreTypeMapping? typeMapping);

    /// <summary>
    /// Creates a <see cref="SelectExpression" /> for <paramref name="entityType" />.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The select expression.</returns>
    SelectExpression Select(IEntityType entityType);
}
