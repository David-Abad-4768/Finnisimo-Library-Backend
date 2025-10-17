using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class LoanRepository(ApplicationDbContext dbContext)
    : BaseRepository<LoanEntity, Guid>(dbContext), ILoanRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public async Task<bool>
  IsBookOnLoanAsync(Guid bookId,
                    CancellationToken cancellationToken = default)
  {
    return await _db.Set<LoanEntity>().AnyAsync(
        l => l.BookId == bookId && l.Status == LoanStatus.Approved,
        cancellationToken);
  }

  public async Task<bool> HasPendingOrActiveLoanForBookAsync(
      Guid userId, Guid bookId, CancellationToken cancellationToken = default)
  {
    return await _db.Set<LoanEntity>().AnyAsync(
        l => l.UserId == userId && l.BookId == bookId &&
             (l.Status == LoanStatus.Requested ||
              l.Status == LoanStatus.Approved),
        cancellationToken);
  }

  public async Task<PaginedResponse<LoanEntity>>
  GetLoansWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize, LoanStatus? status,
      CancellationToken cancellationToken = default)
  {
    IQueryable<LoanEntity> query = _db.Set<LoanEntity>()
                                       .AsNoTracking()
                                       .Include(l => l.Book)
                                       .Where(l => l.UserId == userId);

    if (status.HasValue)
    {
      query = query.Where(l => l.Status == status.Value);
    }

    var orderedQuery = query.OrderByDescending(l => l.LoanedAt);

    var total = await orderedQuery.CountAsync(cancellationToken);

    var items = await orderedQuery.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<LoanEntity>
    {
      Items = items,
      TotalCount = total,
      Page = page,
      PageSize = pageSize
    };
  }

  public async Task<IReadOnlyList<LoanEntity>>
  GetOverdueLoansAsync(CancellationToken cancellationToken = default)
  {
    var now = DateTime.UtcNow;

    return await _db.Set<LoanEntity>()
        .Where(loan => loan.Status == LoanStatus.Approved &&
                       loan.DueDate.HasValue && loan.DueDate.Value < now)
        .ToListAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<LoanEntity>>
  GetLoansDueInRangeAsync(DateTime from, DateTime to,
                          CancellationToken cancellationToken = default)
  {
    return await _db.Set<LoanEntity>()
        .Include(l => l.Book)
        .Where(l => l.DueDate >= from && l.DueDate <= to)
        .ToListAsync(cancellationToken);
  }
}
