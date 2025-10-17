using FluentValidation;

namespace Finnisimo_Library_Backend.Application.Commands.ReadingLogs
    .LogReadingSession;

public class LogReadingSessionCommandValidator
    : AbstractValidator<LogReadingSessionCommand>
{
  public LogReadingSessionCommandValidator()
  {
    RuleFor(x => x.LoanId).NotEmpty().WithMessage("LoanId is required.");

    RuleFor(x => x.StartPage)
        .GreaterThanOrEqualTo(0)
        .WithMessage("Start page must be a non-negative number.");

    RuleFor(x => x.EndPage)
        .GreaterThan(x => x.StartPage)
        .WithMessage("End page must be greater than the start page.");
  }
}
