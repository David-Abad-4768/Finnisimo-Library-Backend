using Finnisimo_Library_Backend.Domain.Entities.Users;

namespace Finnisimo_Library_Backend.Application.Abstractions.Authentication;

public interface IJwtProvider
{

  Task<string> Generate(UserEntity userEntity);
}
