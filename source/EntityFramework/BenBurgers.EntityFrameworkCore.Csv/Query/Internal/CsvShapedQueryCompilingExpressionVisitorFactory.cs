/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

/// <summary>
/// Creates instances of <see cref="CsvShapedQueryCompilingExpressionVisitor" />.
/// </summary>
internal sealed class CsvShapedQueryCompilingExpressionVisitorFactory : IShapedQueryCompilingExpressionVisitorFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvShapedQueryCompilingExpressionVisitorFactory" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvShapedQueryCompilingExpressionVisitorFactory(ShapedQueryCompilingExpressionVisitorDependencies dependencies)
    {
        this.Dependencies = dependencies;
    }

    /// <summary>
    /// Gets the dependencies.
    /// </summary>
    internal ShapedQueryCompilingExpressionVisitorDependencies Dependencies { get; }

    /// <inheritdoc />
    public ShapedQueryCompilingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        => new CsvShapedQueryCompilingExpressionVisitor(this.Dependencies, (CsvQueryCompilationContext)queryCompilationContext);
}
