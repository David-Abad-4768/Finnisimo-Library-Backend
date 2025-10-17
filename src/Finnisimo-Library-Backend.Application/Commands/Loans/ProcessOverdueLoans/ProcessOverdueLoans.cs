using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.Loans
    .ProcessOverdueLoans;

public sealed record ProcessOverdueLoansCommand() : ICommand;
