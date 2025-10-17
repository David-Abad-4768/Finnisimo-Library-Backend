using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.Users.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
  public LoginUserValidator()
  {
    RuleFor(x => x.Username)
        .NotEmpty()
        .WithMessage("Username is required.")
        .MaximumLength(30)
        .WithMessage("Username must not exceed 30 characters.");

    RuleFor(x => x.Password)
        .NotEmpty()
        .WithMessage("Password is required.")
        .MinimumLength(8);
  }
}
