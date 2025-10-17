using ErrorOr;
using AutoMapper;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;
using Finnisimo_Library_Backend.Application.Queries.Responses;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository,
                                              IMapper mapper)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IMapper _mapper = mapper;

  public async Task<ErrorOr<UserResponse>>
  Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
  {

    var user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);

    if (user is null)
    {
      return Error.NotFound(
          code: "User.NotFound",
          description: $"User with ID '{query.Id}' not found.");
    }

    var response = _mapper.Map<UserResponse>(user);

    return response;
  }
}
