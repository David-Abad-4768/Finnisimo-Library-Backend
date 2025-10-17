using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.AcceptExtension;

public sealed record AcceptLoanExtensionCommand(Guid LoanId, Guid UserId)
    : ICommand<string>;
