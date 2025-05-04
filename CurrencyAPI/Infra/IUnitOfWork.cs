using Ardalis.Specification;

namespace CurrencyAPI.Infra;

public interface IUnitOfWork
{
    Task BeginTransaction();
    Task CommitTransaction();
    IRepositoryBase<T> Repository<T>() where T : class;
    Task RollbackTransaction();
}
