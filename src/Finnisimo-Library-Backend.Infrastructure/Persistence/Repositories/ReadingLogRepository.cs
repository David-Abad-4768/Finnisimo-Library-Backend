using Microsoft.EntityFrameworkCore;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class ReadingLogRepository(ApplicationDbContext dbContext)
    : BaseRepository<ReadingLogEntryEntity, Guid>(dbContext),
      IReadingLogRepository
{
  public async Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetLogsForUserInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate,
                              CancellationToken cancellationToken = default)
  {
    return await _dbSet.Include(log => log.Loan)
        .ThenInclude(loan => loan.Book)
        .Where(log => log.UserId == userId && log.DateLogged >= startDate &&
                      log.DateLogged <= endDate &&
                      (log.Loan.Status == LoanStatus.Approved ||
                       log.Loan.Status == LoanStatus.Overdue))
        .ToListAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetLogsForLoanInPeriodAsync(Guid loanId, DateTime startDate, DateTime endDate,
                              CancellationToken cancellationToken = default)
  {
    return await _dbSet
        .Where(log => log.LoanId == loanId && log.DateLogged >= startDate &&
                      log.DateLogged <= endDate)
        .ToListAsync(cancellationToken);
  }

  public async Task<ReadingLogEntryEntity?>
  GetMostRecentLogAsync(Guid userId,
                        CancellationToken cancellationToken = default)
  {
    return await _dbSet.Where(log => log.UserId == userId)
        .OrderByDescending(log => log.DateLogged)
        .FirstOrDefaultAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<ReadingLogEntryEntity>>
  GetFinishedLogsForBooksAsync(Guid userId, DateTime startDate,
                               DateTime endDate,
                               CancellationToken cancellationToken = default)
  {
    var finishedLoansLogs =
        await _dbSet.Include(log => log.Loan)
            .ThenInclude(loan => loan.Book)
            .Where(log => log.UserId == userId && log.DateLogged >= startDate &&
                          log.DateLogged <= endDate)
            .GroupBy(log => log.Loan)
            .Where(group => group.Max(log => log.EndPage) >=
                            group.Key.Book.NumberOfPages)
            .SelectMany(group => group
                                     .Where(log => log.EndPage >=
                                                   group.Key.Book.NumberOfPages)
                                     .OrderBy(log => log.DateLogged)
                                     .Take(1))
            .ToListAsync(cancellationToken);

    return finishedLoansLogs;
  }
}
