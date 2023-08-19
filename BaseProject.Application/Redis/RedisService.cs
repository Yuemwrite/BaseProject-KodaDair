// using Microsoft.Extensions.Configuration;
// using ServiceStack.Redis;
//
// namespace BaseProject.Application.Redis;
//
// public class RedisService 
// {
//     //private readonly RedisConfiguration _redisConfiguration;
//     private readonly IRedisClient _redisClient;
//
//     public RedisService(IConfiguration configuration, IRedisClient redisClient)
//     {
//         _redisClient = redisClient;
//         // _redisConfiguration = configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>();
//     }
//
//     public T? Get<T>(string key, RedisKeyType redisKeyType)
//     {
//         if (Any(key, redisKeyType))
//         {
//             _redisClient.Get<T>(key);
//         }
//
//         return default;
//     }
//
//     public void Add(string key, object data, TimeSpan timeSpan)
//     {
//         _redisClient.Set(key, data, timeSpan);
//     }
//
//     public void Remove(string key)
//     {
//         _redisClient.Remove(key);
//     }
//     
//
//     public bool Any(string key, RedisKeyType redisKeyType)
//     {
//         var keyExistsAsType = _redisClient.GetEntryType(key) == redisKeyType ? true : false;
//         return keyExistsAsType;
//     }
//
//     public RedisClient RedisConnect()
//     {
//         var redisClient = new RedisClient("localhost", 6379);
//
//         return redisClient;
//     }
// }