using Microsoft.EntityFrameworkCore;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;
using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class WishlistItemRepository(ApplicationDbContext dbContext)
    : BaseRepository<WishlistItemEntity, Guid>(dbContext),
      IWishlistItemRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public Task<WishlistItemEntity?>
  GetByUserAndBookAsync(Guid userId, Guid bookId,
                        CancellationToken cancellationToken = default)
  {
    return _db.Set<WishlistItemEntity>().AsNoTracking().FirstOrDefaultAsync(
        wi => wi.UserId == userId && wi.BookId == bookId, cancellationToken);
  }

  public Task<bool>
  AlreadyExistsAsync(Guid userId, Guid bookId,
                     CancellationToken cancellationToken = default)
  {
    return _db.Set<WishlistItemEntity>().AnyAsync(
        wi => wi.UserId == userId && wi.BookId == bookId, cancellationToken);
  }

  public async Task<PaginedResponse<WishlistItemEntity>>
  GetWishlistWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize,
      CancellationToken cancellationToken = default)
  {
    IQueryable<WishlistItemEntity> query = _db.Set<WishlistItemEntity>()
                                               .Include(wi => wi.Book)
                                               .Where(wi => wi.UserId == userId)
                                               .AsNoTracking();

    int totalCount = await query.CountAsync(cancellationToken);

    var items = await query.OrderByDescending(wi => wi.AddedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<WishlistItemEntity>
    {
      Items = items,
      TotalCount = totalCount,
      Page = page,
      PageSize = pageSize
    };
  }
}
