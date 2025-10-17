using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .DeleteWishlistItem;

internal sealed class DeleteWishlistItemCommandHandler(
    IWishlistItemRepository wishlistItemRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteWishlistItemCommand>
{
  public async Task<ErrorOr<None>> Handle(DeleteWishlistItemCommand command,
                                          CancellationToken cancellationToken)
  {
    var wishlistItem = await wishlistItemRepository.GetByUserAndBookAsync(
        command.UserId, command.BookId, cancellationToken);

    if (wishlistItem is null)
    {
      return Error.NotFound(
          "WishlistItem.NotFound",
          "The specified item was not found in the wishlist.");
    }

    await wishlistItemRepository.DeleteAsync(wishlistItem, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return new None();
  }
}
