using Ardalis.Specification;

namespace CurrencyApp.Application.Persistence;

public interface IUnitOfWork
{
    Task BeginTransaction();
    Task CommitTransaction();
    IRepositoryBase<T> Repository<T>() where T : class;
    Task RollbackTransaction();
}