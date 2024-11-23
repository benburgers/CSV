/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

internal sealed class CsvQueryContextFactory : IQueryContextFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryContextFactory" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvQueryContextFactory(QueryContextDependencies dependencies)
    {
        this.Dependencies = dependencies;
    }

    /// <summary>
    /// Gets the dependencies.
    /// </summary>
    internal QueryContextDependencies Dependencies { get; }

    /// <inheritdoc />
    public QueryContext Create() => new CsvQueryContext(this.Dependencies);
}
