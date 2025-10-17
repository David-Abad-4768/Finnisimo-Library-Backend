using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;

public interface IReadingLogRepository
    : IBaseRepository<ReadingLogEntryEntity, Guid>
{
  Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetLogsForUserInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate,
                              CancellationToken cancellationToken = default);

  Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetLogsForLoanInPeriodAsync(Guid loanId, DateTime startDate, DateTime endDate,
                              CancellationToken cancellationToken = default);

  Task<ReadingLogEntryEntity?>
  GetMostRecentLogAsync(Guid userId,
                        CancellationToken cancellationToken = default);

  Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetFinishedLogsForBooksAsync(Guid userId, DateTime startDate,
                               DateTime endDate,
                               CancellationToken cancellationToken = default);
}
