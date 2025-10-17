namespace Finnisimo_Library_Backend.Application.Services.HashedPassword;

public interface IHashedPasswordService
{
  string HashPassword(string password);

  bool VerifyPassword(string password, string hashedPassword);
}
