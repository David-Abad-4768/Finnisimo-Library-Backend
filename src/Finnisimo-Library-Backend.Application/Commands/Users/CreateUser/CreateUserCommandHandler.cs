using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Application.Services.Clock;
using Finnisimo_Library_Backend.Application.Services.HashedPassword;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Users;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository, IUnitOfWork unitOfWork,
    IDateTimeService dateTimeService,
    IHashedPasswordService hashedPasswordService)
    : ICommandHandler<CreateUserCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    if (!await userRepository.IsEmailUniqueAsync(command.Email,
                                                 cancellationToken))
    {
      return Error.Conflict(
          code: "User.EmailInUse",
          description: "The provided email is already in use.");
    }

    if (!await userRepository.IsUsernameUniqueAsync(command.Username,
                                                    cancellationToken))
    {
      return Error.Conflict(
          code: "User.UsernameTaken",
          description: "The provided username is already in use.");
    }

    string photoUrl =
        "https://tse1.mm.bing.net/th/id/" +
        "OIP.yqIudHykN4eXzWxFMTXt0QHaHa?rs=1&pid=ImgDetMain&o=7&rm=3";

    var user = UserEntity.Create(
        command.Name, command.LastName, command.Email, command.Username,
        hashedPasswordService.HashPassword(command.Password), photoUrl,
        command.DateOfBirth.ToUniversalTime(),
        dateTimeService.CurrentTime.ToUniversalTime());

    await userRepository.AddAsync(user, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return "User created successfully";
  }
}
