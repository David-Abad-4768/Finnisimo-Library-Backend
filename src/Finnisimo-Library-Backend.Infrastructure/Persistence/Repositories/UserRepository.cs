using Microsoft.EntityFrameworkCore;
using Finnisimo_Library_Backend.Domain.Entities.Users;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext)
    : BaseRepository<UserEntity, Guid>(dbContext), IUserRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public Task<UserEntity?>
  GetByUsernameAsync(string username,
                     CancellationToken cancellationToken = default)
  {
    return _db.Set<UserEntity>().AsNoTracking().FirstOrDefaultAsync(
        u => u.Username == username, cancellationToken);
  }

  public async Task<bool>
  IsEmailUniqueAsync(string email,
                     CancellationToken cancellationToken = default)
  {
    return !await _db.Set<UserEntity>().AnyAsync(u => u.Email == email,
                                                 cancellationToken);
  }

  public async Task<bool>
  IsUsernameUniqueAsync(string username,
                        CancellationToken cancellationToken = default)
  {
    return !await _db.Set<UserEntity>().AnyAsync(u => u.Username == username,
                                                 cancellationToken);
  }
}
