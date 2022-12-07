/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

internal sealed partial class CsvShapedQueryCompilingExpressionVisitor
{
    private sealed partial class QueryingEnumerable<TCsvRecord, TShape>
    {
        private sealed class Enumerator : IEnumerator<TShape>
        {
            private readonly CsvQueryContext queryContext;
            private readonly Func<CsvQueryContext, TCsvRecord, TShape> shaper;

            /// <summary>
            /// Initializes a new instance of <see cref="Enumerator" />.
            /// </summary>
            /// <param name="queryingEnumerable">The querying enumerable.</param>
            public Enumerator(QueryingEnumerable<TCsvRecord, TShape> queryingEnumerable)
            {
                this.queryContext = queryingEnumerable.queryContext;
                this.shaper = queryingEnumerable.shaper;
            }

            public TShape Current { get; private set; } = default!;

            object IEnumerator.Current => this.Current ?? throw new InvalidOperationException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                EntityFrameworkEventSource.Log.QueryExecuting();
                var csvRecord = Activator.CreateInstance<TCsvRecord>()!;
                var hasNext = false; // TODO determine has next

                this.Current = shaper(this.queryContext, csvRecord);
                return hasNext;
            }

            public void Reset()
                => throw new NotSupportedException(CoreStrings.EnumerableResetNotSupported);
        }
    }
}
