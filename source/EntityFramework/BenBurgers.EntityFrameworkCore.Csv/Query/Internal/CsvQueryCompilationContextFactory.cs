/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

/// <summary>
/// A factory that creates CSV query compilation contexts.
/// </summary>
internal class CsvQueryCompilationContextFactory : IQueryCompilationContextFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryCompilationContextFactory" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvQueryCompilationContextFactory(QueryCompilationContextDependencies dependencies)
    {
        this.Dependencies = dependencies;
    }

    /// <summary>
    /// Gets the dependencies.
    /// </summary>
    public QueryCompilationContextDependencies Dependencies { get; }

    /// <inheritdoc />
    public QueryCompilationContext Create(bool async)
        => new CsvQueryCompilationContext(this.Dependencies, async);
}
