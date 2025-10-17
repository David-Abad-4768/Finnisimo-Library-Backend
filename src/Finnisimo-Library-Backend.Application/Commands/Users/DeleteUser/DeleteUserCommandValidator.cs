using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Users.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
  public DeleteUserCommandValidator()
  {
    RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("User ID is required to perform the deletion.");
  }
}
