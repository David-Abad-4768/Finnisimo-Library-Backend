using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .DeleteWishlistItem;

public record DeleteWishlistItemCommand(Guid UserId, Guid BookId) : ICommand;
