using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.RequestExtension;

public sealed record RequestLoanExtensionCommand(Guid LoanId, Guid UserId,
                                                 int ExtensionInDays)
    : ICommand<string>;
