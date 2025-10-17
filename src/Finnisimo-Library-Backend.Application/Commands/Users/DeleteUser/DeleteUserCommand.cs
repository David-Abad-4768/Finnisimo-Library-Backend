using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand;
