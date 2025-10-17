using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Application.Commands.Users.Responses;

namespace Finnisimo_Library_Backend.Application.Commands.Users.LoginUser;

public record LoginUserCommand(string Username, string Password)
    : ICommand<LoginResult>;
