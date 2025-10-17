namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.User.Request;

public record CreateUserRequest(string Name, string LastName, string Email,
                                string Username, string Password,
                                DateTime DateOfBirth);
