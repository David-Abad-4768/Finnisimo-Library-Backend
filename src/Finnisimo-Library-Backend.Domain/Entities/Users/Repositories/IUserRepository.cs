using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity, Guid>
{
  Task<UserEntity?>
  GetByUsernameAsync(string username,
                     CancellationToken cancellationToken = default);

  Task<bool> IsEmailUniqueAsync(string email,
                                CancellationToken cancellationToken = default);
  Task<bool>
  IsUsernameUniqueAsync(string username,
                        CancellationToken cancellationToken = default);
}
