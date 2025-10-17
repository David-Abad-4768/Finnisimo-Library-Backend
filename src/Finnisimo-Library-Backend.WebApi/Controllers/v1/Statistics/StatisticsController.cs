using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finnisimo_Library_Backend.Application.Commands.ReadingLogs
    .LogReadingSession;
using Finnisimo_Library_Backend.Application.Queries.Statistics.GetFinishedBooks;
using Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetPagesReadPerBook;
using Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetBookReadingFrequency;
using Finnisimo_Library_Backend.WebApi.Controllers.v1.Statistics.Request;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;
using Finnisimo_Library_Backend.Application.Queries.Statistics.GetLastProgress;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Statistics;

[Authorize]
[ApiController]
[Route("api/v1/statistics")]
public class StatisticsController(IMediator mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost("reading-log")]
  public async Task<IActionResult>
  LogReadingSession([FromBody] LogReadingSessionRequest request)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var command = new LogReadingSessionCommand(
        userId, request.LoanId, request.StartPage, request.EndPage);

    var result = await _mediator.Send(command);

    return result.Match(
        _ => Ok(new { message = "Reading session logged successfully" }),
        Problem);
  }

  [HttpGet("pages-per-book")]
  public async Task<IActionResult>
  GetPagesReadPerBook([FromQuery] DateTime targetDate,
                      [FromQuery] TimePeriod period)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var query = new GetPagesReadPerBookQuery(userId, targetDate, period);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("book-frequency/{loanId:guid}")]
  public async Task<IActionResult>
  GetBookReadingFrequency(Guid loanId, [FromQuery] DateTime targetDate,
                          [FromQuery] TimePeriod period)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var query =
        new GetBookReadingFrequencyQuery(userId, loanId, targetDate, period);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("finished-books")]
  public async Task<IActionResult>
  GetFinishedBooks([FromQuery] DateTime targetDate,
                   [FromQuery] TimePeriod period)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var query = new GetFinishedBooksQuery(userId, targetDate, period);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("last-progress/{loanId:guid}")]
  public async Task<IActionResult> GetLastProgress(Guid loanId)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var query = new GetLastProgressQuery(userId, loanId);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }
}
