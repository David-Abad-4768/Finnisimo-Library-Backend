using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.ReadingLogs
    .LogReadingSession;

public record LogReadingSessionCommand(Guid UserId, Guid LoanId, int StartPage,
                                       int EndPage)
    : ICommand;
