using ErrorOr;
using AutoMapper;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Core;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetFinishedBooks;

internal sealed class GetFinishedBooksQueryHandler(
    IReadingLogRepository readingLogRepository, IMapper mapper)
    : IQueryHandler<GetFinishedBooksQuery, IEnumerable<FinishedBookResponse>>
{
  public async Task<ErrorOr<IEnumerable<FinishedBookResponse>>>
  Handle(GetFinishedBooksQuery query, CancellationToken cancellationToken)
  {
    var (startDate, endDate) =
        DateRangeCalculator.Calculate(query.TargetDate, query.Period);

    var finishedLogs = await readingLogRepository.GetFinishedLogsForBooksAsync(
        query.UserId, startDate, endDate, cancellationToken);

    var response = mapper.Map<IEnumerable<FinishedBookResponse>>(finishedLogs);

    return response.ToList();
  }
}
