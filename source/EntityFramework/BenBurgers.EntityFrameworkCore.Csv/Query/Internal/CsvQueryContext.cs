/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Query;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

internal sealed class CsvQueryContext : QueryContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvQueryContext" />.
    /// </summary>
    /// <param name="dependencies">The dependencies.</param>
    public CsvQueryContext(QueryContextDependencies dependencies)
        : base(dependencies)
    {
    }
}
