/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// A binary expression for a CSV record.
/// </summary>
internal sealed partial class CsvBinaryExpression : CsvExpression
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvBinaryExpression" />.
    /// </summary>
    /// <param name="operatorType">The operator type.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="type">The CLR type.</param>
    /// <param name="typeMapping">The type mapping.</param>
    internal CsvBinaryExpression(
        ExpressionType operatorType,
        CsvExpression left,
        CsvExpression right,
        Type type,
        CoreTypeMapping? typeMapping)
        : base(type, typeMapping)
    {
        this.OperatorType = operatorType;
        this.Left = left;
        this.Right = right;
    }

    /// <summary>
    /// Gets the operator type.
    /// </summary>
    internal ExpressionType OperatorType { get; }

    /// <summary>
    /// Gets the left operand.
    /// </summary>
    internal CsvExpression Left { get; }

    /// <summary>
    /// Gets the right operand.
    /// </summary>
    internal CsvExpression Right { get; }

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        var left = (CsvExpression)visitor.Visit(this.Left);
        var right = (CsvExpression)visitor.Visit(this.Right);
        return this.Update(left, right);
    }

    /// <summary>
    /// Updates the <see cref="CsvBinaryExpression" /> to a new <see cref="CsvBinaryExpression" /> with <paramref name="left" /> and <paramref name="right" /> if they are different.
    /// If they are not different the same instance is returned.
    /// </summary>
    /// <param name="left">The left expression.</param>
    /// <param name="right">The right expression.</param>
    /// <returns>A new and updated <see cref="CsvBinaryExpression" /> if <paramref name="left" /> and/or <paramref name="right" /> are different, otherwise the same instance.</returns>
    internal CsvBinaryExpression Update(CsvExpression left, CsvExpression right)
        => !this.Left.Equals(left) || !this.Right.Equals(right)
            ? new CsvBinaryExpression(this.OperatorType, left, right, this.Type, this.TypeMapping)
            : this;
}
