using ErrorOr;
using AutoMapper;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserWishlist;

internal sealed class GetUserWishlistQueryHandler(
    IWishlistItemRepository wishlistItemRepository, IMapper mapper)
    : IQueryHandler<GetUserWishlistQuery,
                    PaginedResponse<UserWishlistResponse>>
{
  public async Task<ErrorOr<PaginedResponse<UserWishlistResponse>>>
  Handle(GetUserWishlistQuery request, CancellationToken cancellationToken)
  {
    var wishlistFromDb =
        await wishlistItemRepository.GetWishlistWithBookDetailsByUserIdAsync(
            request.UserId, request.Page, request.PageSize, cancellationToken);

    var mappedItems =
        mapper.Map<IReadOnlyList<UserWishlistResponse>>(wishlistFromDb.Items);

    var applicationResponse = new PaginedResponse<UserWishlistResponse>
    {
      Items = mappedItems,
      TotalCount = wishlistFromDb.TotalCount,
      Page = wishlistFromDb.Page,
      PageSize = wishlistFromDb.PageSize
    };

    return applicationResponse;
  }
}
