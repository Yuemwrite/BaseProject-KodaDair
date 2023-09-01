using BaseProject.Application.NoSql;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BaseProject.Application.MongoDb;

public class MongoDbService : IMongoDbService
{
    private readonly NoSqlConfiguration _noSqlConfiguration;
    private readonly IConfiguration _configuration;

    public MongoDbService(IConfiguration configuration)
    {
        _configuration = configuration;
        _noSqlConfiguration = _configuration.GetSection("NoSqlConfiguration").Get<NoSqlConfiguration>();
    }

    private async Task<MongoClient> Client()
    {
        var client = new MongoClient(_noSqlConfiguration.MongoDb.ConnectionString);
        return client;
    }

    private async Task<IMongoDatabase> Database()
    {
        var database =  Client().Result.GetDatabase(_noSqlConfiguration.MongoDb.DatabaseName);
        return database;
    }

    public async Task<IMongoCollection<T>> GetCollection<T>(string collectionName)
    {
        var collection = Database().Result.GetCollection<T>(collectionName);
        return collection;
    }

    public async Task<object> Create<T>(T entity, string name)
    {
        var collection = await GetCollection<T>(name);

        return collection.InsertOneAsync(entity);
    }

    public async Task<List<T>> List<T>(string name)
    {
        var collection = await GetCollection<T>(name);

        return await collection.Find(db => true).ToListAsync();
    }
}