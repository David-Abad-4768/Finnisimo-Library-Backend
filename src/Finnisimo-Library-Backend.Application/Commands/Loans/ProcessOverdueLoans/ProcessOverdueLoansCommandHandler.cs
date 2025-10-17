using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans
    .ProcessOverdueLoans;

internal sealed class ProcessOverdueLoansCommandHandler(
    ILoanRepository loanRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ProcessOverdueLoansCommand>
{
  public async Task<ErrorOr<None>> Handle(ProcessOverdueLoansCommand command,
                                          CancellationToken cancellationToken)
  {
    var overdueLoans =
        await loanRepository.GetOverdueLoansAsync(cancellationToken);

    if (!overdueLoans.Any())
    {
      return new None();
    }

    foreach (var loan in overdueLoans)
    {
      loan.MarkAsOverdue();
    }

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
