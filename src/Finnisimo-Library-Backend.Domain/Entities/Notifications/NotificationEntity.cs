using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Notifications;

public sealed class NotificationEntity : BaseEntity
{
  public Guid UserId { get; private set; }
  public string Title { get; private set; }
  public string Message { get; private set; }
  public bool IsRead { get; private set; }
  public DateTime CreatedAt { get; private set; }

  private NotificationEntity(Guid id, Guid userId, string title, string message)
      : base(id)
  {
    UserId = userId;
    Title = title;
    Message = message;
    IsRead = false;
    CreatedAt = DateTime.UtcNow;
  }

  public static NotificationEntity Create(Guid userId, string title,
                                          string message)
  {
    return new NotificationEntity(Guid.NewGuid(), userId, title, message);
  }

  public void MarkAsRead()
  {
    if (!IsRead)
    {
      IsRead = true;
    }
  }
}
