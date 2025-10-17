using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class ReservationRepository(ApplicationDbContext dbContext)
    : BaseRepository<ReservationEntity, Guid>(dbContext),
      IReservationRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public async Task<ReservationEntity?> GetNextActiveReservationForBookAsync(
      Guid bookId, CancellationToken cancellationToken = default)
  {
    return await _db.Set<ReservationEntity>()
        .AsNoTracking()
        .Where(r => r.BookId == bookId && r.Status == ReservationStatus.Active)
        .OrderBy(r => r.ReservationDate)
        .FirstOrDefaultAsync(cancellationToken);
  }

  public async Task<bool>
  HasActiveReservationAsync(Guid userId, Guid bookId,
                            CancellationToken cancellationToken = default)
  {
    return await _db.Set<ReservationEntity>().AnyAsync(
        r => r.UserId == userId && r.BookId == bookId &&
             r.Status == ReservationStatus.Active,
        cancellationToken);
  }

  public async Task<PaginedResponse<ReservationEntity>>
  GetReservationsByUserIdAsync(Guid userId, int page, int pageSize,
                               CancellationToken cancellationToken = default)
  {
    var query = _db.Set<ReservationEntity>()
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ReservationDate);

    var total = await query.CountAsync(cancellationToken);
    var items = await query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<ReservationEntity>
    {
      Items = items,
      TotalCount = total,
      Page = page,
      PageSize = pageSize
    };
  }

  public async Task<ReservationEntity?> GetActiveReservationByUserAndBookAsync(
      Guid userId, Guid bookId, CancellationToken cancellationToken = default)
  {
    return await _db.Set<ReservationEntity>()
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId &&
                                  r.Status == ReservationStatus.Active,
                             cancellationToken);
  }

  public async Task<PaginedResponse<ReservationEntity>>
  GetReservationsWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize, ReservationStatus? status,
      CancellationToken cancellationToken = default)
  {
    IQueryable<ReservationEntity> query = _db.Set<ReservationEntity>()
                                              .AsNoTracking()
                                              .Include(r => r.Book)
                                              .Where(r => r.UserId == userId);

    if (status.HasValue)
    {
      query = query.Where(r => r.Status == status.Value);
    }

    var orderedQuery = query.OrderByDescending(r => r.ReservationDate);

    var total = await orderedQuery.CountAsync(cancellationToken);
    var items = await orderedQuery.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<ReservationEntity>
    {
      Items = items,
      TotalCount = total,
      Page = page,
      PageSize = pageSize
    };
  }

  public async Task<bool> HasActiveReservationsForBookAsync(
      Guid bookId, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<ReservationEntity>().AnyAsync(
        r => r.BookId == bookId && r.Status == ReservationStatus.Active,
        cancellationToken);
  }
}
