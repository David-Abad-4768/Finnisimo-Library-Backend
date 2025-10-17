namespace Finnisimo_Library_Backend.Application.Services.Redis;

public interface IRedisService
{
  Task SetEventAsync(string key, string value, TimeSpan? expiry = null);
  Task<string?> GetEventAsync(string key);
  Task DeleteEventAsync(string key);
}
