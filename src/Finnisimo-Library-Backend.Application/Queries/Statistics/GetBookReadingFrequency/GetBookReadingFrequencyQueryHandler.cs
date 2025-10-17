using ErrorOr;
using Finnisimo_Library_Backend.Application.Core;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetBookReadingFrequency;

internal sealed class GetBookReadingFrequencyQueryHandler(
    IReadingLogRepository readingLogRepository, ILoanRepository loanRepository)
    : IQueryHandler<GetBookReadingFrequencyQuery,
                    IEnumerable<ReadingFrequencyResponse>>
{
  public async Task<ErrorOr<IEnumerable<ReadingFrequencyResponse>>>
  Handle(GetBookReadingFrequencyQuery query,
         CancellationToken cancellationToken)
  {
    var loan =
        await loanRepository.GetByIdAsync(query.LoanId, cancellationToken);
    if (loan is null || loan.UserId != query.UserId)
    {
      return Error.NotFound("Loan.NotFound",
                            "The specified loan was not found.");
    }

    var (startDate, endDate) =
        DateRangeCalculator.Calculate(query.TargetDate, query.Period);

    var readingLogs = await readingLogRepository.GetLogsForLoanInPeriodAsync(
        query.LoanId, startDate, endDate, cancellationToken);

    var stats =
        readingLogs.GroupBy(log => DateOnly.FromDateTime(log.DateLogged))
            .Select(group => new ReadingFrequencyResponse
            {
              Date = group.Key,
              PagesRead = group.Sum(log => log.PagesRead)
            })
            .OrderBy(dto => dto.Date)
            .ToList();

    return stats;
  }
}
