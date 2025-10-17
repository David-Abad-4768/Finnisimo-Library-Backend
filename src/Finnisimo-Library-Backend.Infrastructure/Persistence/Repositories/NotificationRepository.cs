using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Notifications;
using Finnisimo_Library_Backend.Domain.Entities.Notifications.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class NotificationRepository(ApplicationDbContext dbContext)
    : BaseRepository<NotificationEntity, Guid>(dbContext),
      INotificationRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public async Task<PaginedResponse<NotificationEntity>>
  GetByUserIdAsync(Guid userId, int page, int pageSize, bool onlyUnread,
                   CancellationToken cancellationToken = default)
  {
    IQueryable<NotificationEntity> query =
        _db.Set<NotificationEntity>().AsNoTracking().Where(n => n.UserId ==
                                                                userId);

    if (onlyUnread)
    {
      query = query.Where(n => !n.IsRead);
    }

    var orderedQuery = query.OrderByDescending(n => n.CreatedAt);

    var total = await orderedQuery.CountAsync(cancellationToken);

    var items = await orderedQuery.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<NotificationEntity>
    {
      Items = items,
      TotalCount = total,
      Page = page,
      PageSize = pageSize
    };
  }
}
