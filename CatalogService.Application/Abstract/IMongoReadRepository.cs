using CatalogService.Domain.Entities;
using MongoDB.Bson;

namespace CatalogService.Application.Abstract;

public interface IMongoReadRepository<T> where T: MongoDocument
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
}