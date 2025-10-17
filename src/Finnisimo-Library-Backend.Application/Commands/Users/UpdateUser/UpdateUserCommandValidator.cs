using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserCommandValidator()
  {
    RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("User ID is required to perform an update.");

    RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name cannot be empty.")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 characters.");

    RuleFor(x => x.LastName)
        .NotEmpty()
        .WithMessage("Last name cannot be empty.")
        .MaximumLength(50)
        .WithMessage("Last name must not exceed 50 characters.");

    RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Email is required.")
        .EmailAddress()
        .WithMessage("A valid email address is required.");

    RuleFor(x => x.Username)
        .NotEmpty()
        .WithMessage("Username cannot be empty.")
        .MaximumLength(30)
        .WithMessage("Username must not exceed 30 characters.");
  }
}
