using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CancelLoan;

public record CancelLoanCommand(Guid LoanId, Guid UserId) : ICommand<string>;
