/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

/// <summary>
/// A compilation context for CSV queries.
/// </summary>
internal sealed class CsvQueryCompilationContext : QueryCompilationContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryCompilationContext" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    /// <param name="async">Indicates whether the query is asynchronous.</param>
    public CsvQueryCompilationContext(QueryCompilationContextDependencies dependencies, bool async)
        : base(dependencies, async)
    {
    }
}
