using CatalogService.Domain.Entities;

namespace CatalogService.Application.Abstract;

public interface IMongoWriteRepository<T> where T : MongoDocument
{
    Task<T> InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}