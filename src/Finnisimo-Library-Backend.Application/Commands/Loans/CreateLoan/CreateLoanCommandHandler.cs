using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CreateLoan;

internal sealed class CreateLoanCommandHandler(IUserRepository userRepository,
                                               IBookRepository bookRepository,
                                               ILoanRepository loanRepository,
                                               IUnitOfWork unitOfWork)
    : ICommandHandler<CreateLoanCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(CreateLoanCommand command, CancellationToken cancellationToken)
  {
    if (await userRepository.GetByIdAsync(command.UserId, cancellationToken)
            is null)
    {
      return Error.NotFound(
          "User.NotFound",
          $"The user with ID '{command.UserId}' was not found.");
    }

    var book =
        await bookRepository.GetByIdAsync(command.BookId, cancellationToken);
    if (book is null)
    {
      return Error.NotFound(
          "Book.NotFound",
          $"The book with ID '{command.BookId}' was not found.");
    }

    if (await loanRepository.HasPendingOrActiveLoanForBookAsync(
            command.UserId, command.BookId, cancellationToken))
    {
      return Error.Conflict(
          "Loan.DuplicateRequest",
          "You already have a pending or active loan for this book.");
    }

    var loan = LoanEntity.Create(command.BookId, command.UserId);

    if (book.Stock > 0)
    {
      var loanDuration = TimeSpan.FromDays(command.LoanDurationInDays);
      loan.Approve(DateTime.UtcNow, loanDuration);

      book.DecreaseStock(1);
      await bookRepository.UpdateAsync(book, cancellationToken);

      await loanRepository.AddAsync(loan, cancellationToken);
      await unitOfWork.SaveChangesAsync(cancellationToken);

      return "Loan approved successfully.";
    }

    else
    {
      await loanRepository.AddAsync(loan, cancellationToken);

      await unitOfWork.SaveChangesAsync(cancellationToken);

      return "The book is currently out of stock. Your request has been " +
             "placed in a queue.";
    }
  }
}
