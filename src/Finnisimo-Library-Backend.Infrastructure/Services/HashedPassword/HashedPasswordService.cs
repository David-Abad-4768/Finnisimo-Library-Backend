using Finnisimo_Library_Backend.Application.Services.HashedPassword;

namespace Finnisimo_Library_Backend.Infrastructure.Services.HashedPassword;

internal sealed class HashedPasswordService : IHashedPasswordService
{
  public string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }

  public bool VerifyPassword(string password, string hashedPassword)
  {
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}
