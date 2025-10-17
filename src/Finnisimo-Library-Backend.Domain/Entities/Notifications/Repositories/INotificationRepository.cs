using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Notifications.Repositories;

public interface INotificationRepository
    : IBaseRepository<NotificationEntity, Guid>
{
  Task<PaginedResponse<NotificationEntity>>
  GetByUserIdAsync(Guid userId, int page, int pageSize, bool onlyUnread,
                   CancellationToken cancellationToken = default);
}
