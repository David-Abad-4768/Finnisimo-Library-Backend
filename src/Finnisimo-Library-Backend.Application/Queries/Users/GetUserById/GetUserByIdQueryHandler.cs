using ErrorOr;
using AutoMapper;
using System.Text.Json;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;
using Finnisimo_Library_Backend.Application.Services.Redis;
using Finnisimo_Library_Backend.Application.Queries.Responses;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository,
                                              IMapper mapper,
                                              IRedisService redisService)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IMapper _mapper = mapper;
  private readonly IRedisService _redisService = redisService;

  public async Task<ErrorOr<UserResponse>>
  Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
  {
    var cacheKey = $"user:{query.Id}";

    var cachedResponse = await _redisService.GetEventAsync(cacheKey);

    if (cachedResponse is not null)
    {
      var cachedUser = JsonSerializer.Deserialize<UserResponse>(cachedResponse);
      return cachedUser!;
    }

    var user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);

    if (user is null)
    {
      return Error.NotFound(
          code: "User.NotFound",
          description: $"User with ID '{query.Id}' not found.");
    }

    var response = _mapper.Map<UserResponse>(user);

    var serializedResponse = JsonSerializer.Serialize(response);
    await _redisService.SetEventAsync(cacheKey, serializedResponse);

    return response;
  }
}
