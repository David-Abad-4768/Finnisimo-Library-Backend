using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Authentication;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Application.Commands.Users.Responses;
using Finnisimo_Library_Backend.Application.Services.HashedPassword;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Users.LoginUser;

internal sealed class LoginUserCommandHandler(IUserRepository userRepository,
                                              IHashedPasswordService pwdService,
                                              IJwtProvider jwtProvider)
    : ICommandHandler<LoginUserCommand, LoginResult>
{
  private readonly IUserRepository _userRepository = userRepository;
  private readonly IHashedPasswordService _pwdService = pwdService;
  private readonly IJwtProvider _jwtProvider = jwtProvider;

  public async Task<ErrorOr<LoginResult>>
  Handle(LoginUserCommand command, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByUsernameAsync(command.Username,
                                                        cancellationToken);

    if (user is null ||
        !_pwdService.VerifyPassword(command.Password, user.Password))
    {
      return Error.Validation(description: "Invalid credentials.");
    }

    var token = await _jwtProvider.Generate(user);

    var loginResult = new LoginResult(token, "Login successfully",
                                      DateTime.UtcNow.AddMinutes(10));

    return loginResult;
  }
}
