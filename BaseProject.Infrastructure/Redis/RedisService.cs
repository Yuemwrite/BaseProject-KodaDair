using BaseProject.Application.Redis;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;

namespace BaseProject.Infrastructure.Redis;

public class RedisService : IRedisService
{
    readonly RedisClient _redisClient = new RedisClient();

    public RedisService(IConfiguration configuration)
    {
        RedisConnect(configuration);
    }

    public T Get<T>(string key)
    {
        return _redisClient.Get<T>(key);
    }

    public void Add(string key, object data, TimeSpan timeSpan)
    {
        _redisClient.Set(key, data, timeSpan);
    }

    public void Remove(string key)
    {
        _redisClient.Remove(key);
    }

    public bool Any(string key, RedisKeyType redisKeyType)
    {
        throw new NotImplementedException();
    }


    public bool Any(string key)
    {
        return true;
    }

    private RedisClient RedisConnect(IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>();

        var redisClient = new RedisClient(redisConfiguration!.Host, redisConfiguration!.Port);


        return redisClient;
    }
}