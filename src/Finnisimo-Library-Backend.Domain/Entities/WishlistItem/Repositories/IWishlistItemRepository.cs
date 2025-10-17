using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;

public interface IWishlistItemRepository
    : IBaseRepository<WishlistItemEntity, Guid>
{
  Task<WishlistItemEntity?>
  GetByUserAndBookAsync(Guid userId, Guid bookId,
                        CancellationToken cancellationToken = default);

  Task<bool> AlreadyExistsAsync(Guid userId, Guid bookId,
                                CancellationToken cancellationToken = default);

  Task<PaginedResponse<WishlistItemEntity>>
  GetWishlistWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize,
      CancellationToken cancellationToken = default);
}
