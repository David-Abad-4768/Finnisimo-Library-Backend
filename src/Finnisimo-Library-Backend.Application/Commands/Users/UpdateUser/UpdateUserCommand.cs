using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Users.UpdateUser;

public record UpdateUserCommand(Guid UserId, string Name, string LastName,
                                string Email, string Username)
    : ICommand;
