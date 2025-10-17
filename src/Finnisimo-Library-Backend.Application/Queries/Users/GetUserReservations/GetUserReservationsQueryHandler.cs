using AutoMapper;
using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Users
    .GetUserReservations;

internal sealed class GetUserReservationsQueryHandler(
    IReservationRepository reservationRepository, IMapper mapper)
    : IQueryHandler<GetUserReservationsQuery,
                    PaginedResponse<UserReservationResponse>>
{
  public async Task<ErrorOr<PaginedResponse<UserReservationResponse>>>
  Handle(GetUserReservationsQuery request,
         CancellationToken cancellationToken)
  {
    PaginedResponse<ReservationEntity> reservationsFromDb =
        await reservationRepository.GetReservationsWithBookDetailsByUserIdAsync(
            request.UserId, request.Page, request.PageSize, request.Status,
            cancellationToken);

    var mappedItems = mapper.Map<IReadOnlyList<UserReservationResponse>>(
        reservationsFromDb.Items);

    var applicationResponse = new PaginedResponse<UserReservationResponse>
    {
      Items = mappedItems,
      TotalCount = reservationsFromDb.TotalCount,
      Page = reservationsFromDb.Page,
      PageSize = reservationsFromDb.PageSize
    };

    return applicationResponse;
  }
}
