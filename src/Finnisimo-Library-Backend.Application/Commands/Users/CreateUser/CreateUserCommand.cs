using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Users.CreateUser;

public record CreateUserCommand(string Name, string LastName, string Email,
                                string Username, string Password,
                                DateTime DateOfBirth)
    : ICommand<string>;
