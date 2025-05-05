using Ardalis.Specification;

namespace CurrencyApp.Application.Persistence;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}