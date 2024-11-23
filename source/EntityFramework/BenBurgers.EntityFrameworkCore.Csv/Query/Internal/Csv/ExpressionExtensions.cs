/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// Extension methods for expressions.
/// </summary>
internal static class ExpressionExtensions
{
    /// <summary>
    /// Infer a type mapping from the provided <paramref name="expressions" />.
    /// </summary>
    /// <param name="expressions">The expressions to infer the type mapping from.</param>
    /// <returns>The inferred type mapping.</returns>
    internal static CoreTypeMapping? InferTypeMapping(params Expression[] expressions)
    {
        for (var i = 0; i < expressions.Length; i++)
        {
            if (expressions[i] is CsvExpression { TypeMapping: { } typeMapping })
                return typeMapping;
        }
        return null;
    }

    /// <summary>
    /// Gets a constant value of type <typeparamref name="T" /> from an expression.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <returns>The constant value.</returns>
    /// <exception cref="InvalidOperationException">An <see cref="InvalidOperationException" /> is thrown if the constant value could not be found.</exception>
    public static T GetConstantValue<T>(this Expression expression)
        => expression is ConstantExpression { Value: T { } value }
            ? value
            : throw new InvalidOperationException();
}
