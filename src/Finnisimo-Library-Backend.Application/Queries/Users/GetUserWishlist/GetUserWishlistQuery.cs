using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserWishlist;

public sealed record GetUserWishlistQuery(Guid UserId, int Page, int PageSize)
    : IQuery<PaginedResponse<UserWishlistResponse>>;
