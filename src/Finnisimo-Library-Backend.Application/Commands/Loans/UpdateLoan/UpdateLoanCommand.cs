using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Entities.Loans;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.UpdateLoan;

public record UpdateLoanCommand(Guid LoanId, LoanStatus Status,
                                TimeSpan? LoanDuration = null)
    : ICommand;
