using ApiGateway.Domain.Entities;

namespace ApiGateway.Application.Abstract;

public interface IMongoRepository<T> where T: MongoDocument
{
    Task<List<T>> GetAllRecords();
    T InsertRecords(T record);
    T GetRecordById(Guid id);
    void UpsertRecord(T record);
    void DeleteRecord(T record);
}