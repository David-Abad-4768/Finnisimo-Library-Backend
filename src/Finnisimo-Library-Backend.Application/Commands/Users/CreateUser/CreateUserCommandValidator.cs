using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Users.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
  public CreateUserCommandValidator()
  {
    RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name is required.")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 characters.");

    RuleFor(x => x.LastName)
        .NotEmpty()
        .WithMessage("Last name is required.")
        .MaximumLength(50)
        .WithMessage("Last name must not exceed 50 characters.");

    RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Email is required.")
        .EmailAddress()
        .WithMessage("A valid email address is required.");

    RuleFor(x => x.Username)
        .NotEmpty()
        .WithMessage("Username is required.")
        .MaximumLength(30)
        .WithMessage("Username must not exceed 30 characters.");

    RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Password is required.")
        .MinimumLength(8)
        .WithMessage("Password must be at least 8 characters.")
        .Matches(@"[A-Z]")
        .WithMessage("Password must contain at least one uppercase letter.")
        .Matches(@"[a-z]")
        .WithMessage("Password must contain at least one lowercase letter.")
        .Matches(@"\d")
        .WithMessage("Password must contain at least one number.")
        .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+]")
        .WithMessage("Password must contain at least one special character " +
                     "(!, @, #, $, etc.).");

    RuleFor(x => x.DateOfBirth)
        .NotEmpty()
        .WithMessage("Date of birth is required.")
        .LessThan(DateTime.Today)
        .WithMessage("Date of birth must be in the past.");
  }
}
