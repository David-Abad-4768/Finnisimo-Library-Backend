using ErrorOr;
using Finnisimo_Library_Backend.Application.Core;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetPagesReadPerBook;

internal sealed class GetPagesReadPerBookQueryHandler(
    IReadingLogRepository readingLogRepository)
    : IQueryHandler<GetPagesReadPerBookQuery,
                    IEnumerable<BookPagesReadResponse>>
{
  public async Task<ErrorOr<IEnumerable<BookPagesReadResponse>>>
  Handle(GetPagesReadPerBookQuery query, CancellationToken cancellationToken)
  {
    var (startDate, endDate) =
        DateRangeCalculator.Calculate(query.TargetDate, query.Period);

    var readingLogs = await readingLogRepository.GetLogsForUserInPeriodAsync(
        query.UserId, startDate, endDate, cancellationToken);

    var stats =
        readingLogs.GroupBy(log => log.Loan.Book.Title)
            .Select(group => new BookPagesReadResponse
            {
              Title = group.Key,
              PagesRead = group.Sum(log => log.PagesRead)
            })
            .ToList();

    return stats;
  }
}
