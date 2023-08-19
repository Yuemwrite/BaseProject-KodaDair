using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;

namespace BaseProject.Application.Redis;

public interface IRedisService
{
    T? Get<T>(string key);
    void Add(string key, object data, TimeSpan timeSpan);
    void Remove(string key);
    bool Any(string key, RedisKeyType redisKeyType);
    
}