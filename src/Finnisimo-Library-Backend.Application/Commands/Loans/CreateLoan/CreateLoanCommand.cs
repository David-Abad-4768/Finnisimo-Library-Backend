using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CreateLoan;

public record CreateLoanCommand(Guid UserId, Guid BookId,
                                int LoanDurationInDays)
    : ICommand<string>;
