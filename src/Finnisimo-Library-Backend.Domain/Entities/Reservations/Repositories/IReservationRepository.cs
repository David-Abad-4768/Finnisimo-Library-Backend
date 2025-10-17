using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;

public interface IReservationRepository
    : IBaseRepository<ReservationEntity, Guid>
{
  Task<ReservationEntity?> GetNextActiveReservationForBookAsync(
      Guid bookId, CancellationToken cancellationToken = default);

  Task<bool>
  HasActiveReservationAsync(Guid userId, Guid bookId,
                            CancellationToken cancellationToken = default);

  Task<PaginedResponse<ReservationEntity>>
  GetReservationsByUserIdAsync(Guid userId, int page, int pageSize,
                               CancellationToken cancellationToken = default);

  Task<ReservationEntity?> GetActiveReservationByUserAndBookAsync(
      Guid userId, Guid bookId, CancellationToken cancellationToken = default);

  Task<PaginedResponse<ReservationEntity>>
  GetReservationsWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize, ReservationStatus? status,
      CancellationToken cancellationToken = default);

  Task<bool> HasActiveReservationsForBookAsync(
      Guid bookId, CancellationToken cancellationToken = default);
}
