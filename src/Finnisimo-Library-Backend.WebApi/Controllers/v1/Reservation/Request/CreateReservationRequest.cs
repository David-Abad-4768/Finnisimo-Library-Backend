namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Reservation.Request;

public sealed class CreateReservationRequest
{
  public Guid BookId { get; set; }
  public int DesiredLoanDurationInDays { get; set; }
}
