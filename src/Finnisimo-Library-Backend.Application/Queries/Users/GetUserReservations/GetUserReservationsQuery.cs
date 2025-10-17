using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;

namespace Finnisimo_Library_Backend.Application.Queries.Users
    .GetUserReservations;

public sealed record GetUserReservationsQuery(Guid UserId, int Page,
                                              int PageSize,
                                              ReservationStatus? Status)
    : IQuery<PaginedResponse<UserReservationResponse>>;
