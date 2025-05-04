using Ardalis.Specification;
using CurrencyAPI.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace CurrencyAPI.Infra;


public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork, IDisposable
{
    public IRepositoryBase<T> Repository<T>() where T : class => new Repository<T>(dbContext);
    private IDbContextTransaction transaction;

    public async Task BeginTransaction()
    {
        if (transaction != null)
        {
            throw new InvalidOperationException("Transaction already started.");
        }

        transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
    }

    public async Task CommitTransaction()
    {
        await transaction.CommitAsync();
        transaction.Dispose();
        transaction = null;
    }

    public async Task RollbackTransaction()
    {
        await transaction.RollbackAsync();
        transaction.Dispose();
        transaction = null;
    }

    public void Dispose()
    {
        dbContext.Dispose();
        transaction?.Dispose();
    }
}