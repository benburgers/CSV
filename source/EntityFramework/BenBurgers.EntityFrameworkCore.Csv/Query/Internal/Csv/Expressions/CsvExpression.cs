/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression for a CSV record.
/// </summary>
internal abstract partial class CsvExpression : Expression
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvExpression" />.
    /// </summary>
    /// <param name="type">The CLR type.</param>
    /// <param name="typeMapping">The type mapping.</param>
    protected CsvExpression(Type type, CoreTypeMapping? typeMapping)
    {
        this.Type = type;
        this.TypeMapping = typeMapping;
    }

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <inheritdoc />
    public override Type Type { get; }

    /// <summary>
    /// Gets the type mapping.
    /// </summary>
    internal CoreTypeMapping? TypeMapping { get; }
}
