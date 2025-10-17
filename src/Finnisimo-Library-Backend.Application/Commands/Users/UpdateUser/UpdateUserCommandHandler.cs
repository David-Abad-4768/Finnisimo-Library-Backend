using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(IUserRepository userRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
  public async Task<ErrorOr<None>> Handle(UpdateUserCommand command,
                                          CancellationToken cancellationToken)
  {
    var user =
        await userRepository.GetByIdAsync(command.UserId, cancellationToken);

    if (user is null)
    {
      return Error.NotFound(
          code: "User.NotFound",
          description: $"The user with ID '{command.UserId}' was not found.");
    }

    if (user.Email != command.Email && !await userRepository.IsEmailUniqueAsync(
                                           command.Email, cancellationToken))
    {
      return Error.Conflict(code: "User.EmailInUse",
                            description: "The provided email is already in " +
                                "use by another account.");
    }

    if (user.Username != command.Username &&
        !await userRepository.IsUsernameUniqueAsync(command.Username,
                                                    cancellationToken))
    {
      return Error.Conflict(code: "User.UsernameTaken",
                            description: "The provided username is already " +
                                "in use by another account.");
    }

    user.Update(command.Name, command.LastName, command.Email,
                command.Username);

    await userRepository.UpdateAsync(user, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return default;
  }
}
