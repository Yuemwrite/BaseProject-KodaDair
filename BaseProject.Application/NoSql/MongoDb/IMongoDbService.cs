using MongoDB.Driver;

namespace BaseProject.Application.MongoDb;

public interface IMongoDbService
{
  Task<IMongoCollection<T>> GetCollection<T>(string collectionName);

  Task<object> Create<T>(T entity, string name);

  Task<List<T>> List<T>(string name);

}