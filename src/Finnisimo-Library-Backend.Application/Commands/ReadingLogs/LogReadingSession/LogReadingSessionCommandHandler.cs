using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.ReadingLogs
    .LogReadingSession;

internal sealed class LogReadingSessionCommandHandler(
    IReadingLogRepository readingLogRepository, ILoanRepository loanRepository,
    IBookRepository bookRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<LogReadingSessionCommand>
{
  public async Task<ErrorOr<None>> Handle(LogReadingSessionCommand command,
                                          CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(command.LoanId, cancellationToken);

    if (loan is null || loan.UserId != command.UserId)
    {
      return Error.NotFound("Loan.NotFound",
                            "The specified loan was not found for this user.");
    }

    if (loan.Status != LoanStatus.Approved &&
        loan.Status != LoanStatus.Overdue)
    {
      return Error.Validation(
          "Loan.NotActive",
          "Reading can only be logged for active or overdue loans.");
    }

    var book =
        await bookRepository.GetByIdAsync(loan.BookId, cancellationToken);
    if (book is null)
    {
      return Error.NotFound("Book.NotFound",
                            "The associated book was not found.");
    }

    if (command.EndPage > book.NumberOfPages)
    {
      return Error.Validation(
          "ReadingLog.InvalidPage",
          $"End page ({command.EndPage}) cannot be greater than the total number of pages ({book.NumberOfPages}).");
    }

    var logEntry = ReadingLogEntryEntity.Create(
        command.LoanId, command.UserId, command.StartPage, command.EndPage);

    await readingLogRepository.AddAsync(logEntry, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
