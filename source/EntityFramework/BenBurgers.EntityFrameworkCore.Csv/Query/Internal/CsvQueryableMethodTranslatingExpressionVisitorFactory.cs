/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

/// <summary>
/// Creates instances of <see cref="CsvQueryableMethodTranslatingExpressionVisitor" />.
/// </summary>
internal sealed class CsvQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryableMethodTranslatingExpressionVisitorFactory" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <param name="csvExpressionFactory">The CSV expression factory.</param>
    public CsvQueryableMethodTranslatingExpressionVisitorFactory(
        QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
        ICsvExpressionFactory csvExpressionFactory)
    {
        this.Dependencies = dependencies;
        this.CsvExpressionFactory = csvExpressionFactory;
    }

    /// <summary>
    /// Gets the main dependencies.
    /// </summary>
    internal QueryableMethodTranslatingExpressionVisitorDependencies Dependencies { get; }

    /// <summary>
    /// Gets the CSV expression factory.
    /// </summary>
    internal ICsvExpressionFactory CsvExpressionFactory { get; }

    /// <inheritdoc />
    public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        => new CsvQueryableMethodTranslatingExpressionVisitor(this.Dependencies, queryCompilationContext, this.CsvExpressionFactory);
}
