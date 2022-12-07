/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal;

internal sealed partial class CsvShapedQueryCompilingExpressionVisitor
{
    private sealed partial class QueryingEnumerable<TCsvRecord, TShape> : IEnumerable<TShape>, IAsyncEnumerable<TShape>, IQueryingEnumerable
    {
        private readonly CsvQueryContext queryContext;
        private readonly SelectExpression selectExpression;
        private readonly Func<CsvQueryContext, TCsvRecord, TShape> shaper;

        /// <summary>
        /// Initializes a new instance of <see cref="QueryingEnumerable{TCsvRecord, TShape}" />.
        /// </summary>
        /// <param name="queryContext">The query context.</param>
        /// <param name="selectExpression">The select expression.</param>
        /// <param name="shaper">The shaper function.</param>
        public QueryingEnumerable(
            CsvQueryContext queryContext,
            SelectExpression selectExpression,
            Func<CsvQueryContext, TCsvRecord, TShape> shaper)
        {
            this.queryContext = queryContext;
            this.selectExpression = selectExpression;
            this.shaper = shaper;
        }

        /// <inheritdoc />
        public IAsyncEnumerator<TShape> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerator<TShape> GetEnumerator()
            => new Enumerator(this);

        /// <inheritdoc />
        public string ToQueryString()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
