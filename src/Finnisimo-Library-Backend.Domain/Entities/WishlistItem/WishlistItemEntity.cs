using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Users;

namespace Finnisimo_Library_Backend.Domain.Entities.WishlistItem;

public sealed class WishlistItemEntity : BaseEntity
{
  private WishlistItemEntity(Guid id, Guid userId, Guid bookId,
                             DateTime addedAt)
      : base(id)
  {
    UserId = userId;
    BookId = bookId;
    AddedAt = addedAt;
  }

  public Guid UserId { get; private set; }
  public Guid BookId { get; private set; }
  public DateTime AddedAt { get; private set; }

  public UserEntity User { get; private set; } = null!;
  public BookEntity Book { get; private set; } = null!;

  public static WishlistItemEntity Create(Guid userId, Guid bookId)
  {
    var wishlistItem =
        new WishlistItemEntity(Guid.NewGuid(), userId, bookId, DateTime.UtcNow);

    return wishlistItem;
  }
}
