using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using CurrencyApp.Data;

namespace CurrencyAPI.Infra;
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}

public class Repository<T>(AppDbContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T> where T : class
{
}