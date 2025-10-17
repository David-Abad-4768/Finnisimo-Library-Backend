using StackExchange.Redis;
using Finnisimo_Library_Backend.Application.Services.Redis;

namespace Finnisimo_Library_Backend.Infrastructure.Services.Redis;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
  private readonly IDatabase _db = redis.GetDatabase();

  public Task SetEventAsync(string key, string value,
                            TimeSpan? expiry = null) =>
      _db.StringSetAsync(key, value, expiry);

  public async Task<string?>
  GetEventAsync(string key) => await _db.StringGetAsync(key);

  public Task DeleteEventAsync(string key) => _db.KeyDeleteAsync(key);
}
