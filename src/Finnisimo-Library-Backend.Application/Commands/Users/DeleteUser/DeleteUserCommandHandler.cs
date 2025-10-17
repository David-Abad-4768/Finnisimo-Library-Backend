using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler(IUserRepository userRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUserCommand>
{
  private readonly IUserRepository _userRepository = userRepository;

  public async Task<ErrorOr<None>> Handle(DeleteUserCommand command,
                                          CancellationToken cancellationToken)
  {
    var user =
        await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

    if (user is null)
      return Error.NotFound(
          description: $"User with ID {command.UserId} not found.");

    await _userRepository.DeleteAsync(user, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
