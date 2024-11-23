/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;

namespace BenBurgers.EntityFrameworkCore.Csv.Storage.Internal;

internal sealed class CsvDbContextTransactionManager : IDbContextTransactionManager, ITransactionEnlistmentManager
{
    public IDbContextTransaction? CurrentTransaction => throw new NotImplementedException();

    public Transaction? EnlistedTransaction => throw new NotImplementedException();

    public IDbContextTransaction BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void CommitTransaction()
    {
        throw new NotImplementedException();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void EnlistTransaction(Transaction? transaction)
    {
        throw new NotImplementedException();
    }

    public void ResetState()
    {
        throw new NotImplementedException();
    }

    public Task ResetStateAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
