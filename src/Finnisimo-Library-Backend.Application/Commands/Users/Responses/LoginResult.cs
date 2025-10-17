namespace Finnisimo_Library_Backend.Application.Commands.Users.Responses;

public record LoginResult(string AccessToken, string Message,
                          DateTime ExpirationDate);
