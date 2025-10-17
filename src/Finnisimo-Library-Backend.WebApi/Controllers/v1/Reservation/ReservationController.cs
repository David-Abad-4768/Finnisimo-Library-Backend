using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finnisimo_Library_Backend.Application.Commands.Reservations
    .CancelReservation;
using Finnisimo_Library_Backend.WebApi.Controllers.v1.Reservation.Request;
using Finnisimo_Library_Backend.Application.Commands.Reservations
    .CreateReservation;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Reservation;

[Authorize]
[ApiController]
[Route("api/v1/reservations")]
public class ReservationsController(IMediator mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult>
  CreateReservation([FromBody] CreateReservationRequest request)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var command = new CreateReservationCommand(
        request.BookId, userId, request.DesiredLoanDurationInDays);
    var result = await _mediator.Send(command);

    return result.Match(reservationId =>
                            CreatedAtAction(nameof(CreateReservation),
                                            new { id = reservationId }, null),
                        Problem);
  }

  [HttpDelete("{reservationId:guid}")]
  public async Task<IActionResult> CancelReservation(Guid reservationId)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var command = new CancelReservationCommand(reservationId, userId);
    var result = await _mediator.Send(command);

    return result.Match(
        _ => NoContent(), Problem);
  }
}
