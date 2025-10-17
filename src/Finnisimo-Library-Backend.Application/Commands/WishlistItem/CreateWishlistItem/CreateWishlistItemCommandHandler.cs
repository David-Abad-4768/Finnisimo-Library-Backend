using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .CreateWishlistItem;

internal sealed class CreateWishlistItemCommandHandler(
    IUserRepository userRepository, IBookRepository bookRepository,
    IWishlistItemRepository wishlistItemRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateWishlistItemCommand, string>
{
  public async Task<ErrorOr<string>>
  Handle(CreateWishlistItemCommand command,
         CancellationToken cancellationToken)
  {
    var user =
        await userRepository.GetByIdAsync(command.UserId, cancellationToken);
    if (user is null)
    {
      return Error.NotFound("User.NotFound",
                            "The specified user was not found.");
    }

    var book =
        await bookRepository.GetByIdAsync(command.BookId, cancellationToken);
    if (book is null)
    {
      return Error.NotFound("Book.NotFound",
                            "The specified book was not found.");
    }

    var alreadyExists = await wishlistItemRepository.AlreadyExistsAsync(
        command.UserId, command.BookId, cancellationToken);
    if (alreadyExists)
    {
      return Error.Conflict("WishlistItem.AlreadyExists",
                            "This book is already in the user's wishlist.");
    }

    var wishlistItem =
        WishlistItemEntity.Create(command.UserId, command.BookId);

    await wishlistItemRepository.AddAsync(wishlistItem, cancellationToken);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return "Book added to wishlist successfully.";
  }
}
