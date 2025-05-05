using Ardalis.Specification.EntityFrameworkCore;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data;

namespace CurrencyApp.Infra.Persistence;

public class Repository<T>(AppDbContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T> where T : class
{
}