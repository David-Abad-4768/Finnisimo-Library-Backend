using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetLastProgress;

internal sealed class GetLastProgressQueryHandler(
    IReadingLogRepository readingLogRepository, ILoanRepository loanRepository)
    : IQueryHandler<GetLastProgressQuery, LastProgressResponse>
{
  public async Task<ErrorOr<LastProgressResponse>>
  Handle(GetLastProgressQuery query, CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(query.LoanId, cancellationToken);
    if (loan is null || loan.UserId != query.UserId)
    {
      return Error.NotFound("Loan.NotFound",
                            "The specified loan was not found for this user.");
    }

    var logs = await readingLogRepository.GetLogsForLoanInPeriodAsync(
        query.LoanId, DateTime.MinValue, DateTime.MaxValue, cancellationToken);

    if (!logs.Any())
    {
      return new LastProgressResponse { LastPage = 0 };
    }

    int lastPage = logs.Max(l => l.EndPage);

    return new LastProgressResponse { LastPage = lastPage };
  }
}
