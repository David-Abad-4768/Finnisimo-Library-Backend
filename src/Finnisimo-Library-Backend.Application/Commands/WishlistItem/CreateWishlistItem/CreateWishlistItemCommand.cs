using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .CreateWishlistItem;

public record CreateWishlistItemCommand(Guid UserId, Guid BookId)
    : ICommand<string>;
