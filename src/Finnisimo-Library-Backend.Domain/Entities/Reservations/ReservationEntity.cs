using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;
using Finnisimo_Library_Backend.Domain.Entities.Users;

namespace Finnisimo_Library_Backend.Domain.Entities.Reservations;

public sealed class ReservationEntity : BaseEntity
{
  public Guid BookId { get; private set; }
  public Guid UserId { get; private set; }
  public DateTime ReservationDate { get; private set; }
  public ReservationStatus Status { get; private set; }
  public int DesiredLoanDurationInDays { get; private set; }

  private ReservationEntity(Guid id, Guid bookId, Guid userId,
                            int desiredLoanDurationInDays)
      : base(id)
  {
    BookId = bookId;
    UserId = userId;
    ReservationDate = DateTime.UtcNow;
    Status = ReservationStatus.Active;
    DesiredLoanDurationInDays = desiredLoanDurationInDays;
  }

  public static ReservationEntity Create(Guid bookId, Guid userId,
                                         int desiredLoanDurationInDays)
  {
    var reservation = new ReservationEntity(Guid.NewGuid(), bookId, userId,
                                            desiredLoanDurationInDays);

    reservation.RaiseDomainEvent(new ReservationCreatedDomainEvent(
        reservation.Id, reservation.UserId, reservation.BookId));

    return reservation;
  }

  public void Fulfill()
  {
    if (Status != ReservationStatus.Active)
    {
      throw new InvalidOperationException(
          "Only an active reservation can be fulfilled.");
    }
    Status = ReservationStatus.Fulfilled;

    RaiseDomainEvent(new ReservationFulfilledDomainEvent(Id, BookId, UserId));
  }

  public void Cancel()
  {
    if (Status != ReservationStatus.Active)
    {
      throw new InvalidOperationException(
          "Only an active reservation can be cancelled.");
    }
    Status = ReservationStatus.Cancelled;

    RaiseDomainEvent(new ReservationCancelledDomainEvent(Id, BookId));
  }

  public void Expire()
  {
    if (Status != ReservationStatus.Active)
    {
      throw new InvalidOperationException(
          "Only an active reservation can expire.");
    }
    Status = ReservationStatus.Expired;
  }

  public BookEntity Book { get; private set; } = null!;
  public UserEntity User { get; private set; } = null!;
}
