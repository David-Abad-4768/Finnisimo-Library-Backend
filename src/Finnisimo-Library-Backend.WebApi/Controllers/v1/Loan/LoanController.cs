using MediatR;
using ErrorOr;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finnisimo_Library_Backend.Application.Commands.Loans.CreateLoan;
using Finnisimo_Library_Backend.Application.Commands.Loans.CancelLoan;
using Finnisimo_Library_Backend.Application.Commands.Loans.AcceptExtension;
using Finnisimo_Library_Backend.WebApi.Controllers.v1.Loan.Request;
using Finnisimo_Library_Backend.Application.Commands.Loans.RequestExtension;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Loan;

[Authorize]
[ApiController]
[Route("api/v1/loans")]
public class LoansController(ISender sender) : ApiController
{
  private readonly ISender _sender = sender;

  [HttpPost]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails),
                        StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ProblemDetails),
                        StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult>
  RequestLoan([FromBody] CreateLoanRequest request)
  {
    var userIdResult = GetUserId();
    if (userIdResult.IsError)
      return Problem(userIdResult.Errors);

    var command = new CreateLoanCommand(userIdResult.Value, request.BookId,
                                        request.LoanDurationInDays);
    var result = await _sender.Send(command);

    return result.Match(message => Ok(new { Message = message }), Problem);
  }

  [HttpDelete("{loanId:guid}")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails),
                        StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> CancelLoan(Guid loanId)
  {
    var userIdResult = GetUserId();
    if (userIdResult.IsError)
      return Problem(userIdResult.Errors);

    var command = new CancelLoanCommand(loanId, userIdResult.Value);
    var result = await _sender.Send(command);

    return result.Match(message => Ok(new { Message = message }), Problem);
  }

  [HttpPost("{loanId:guid}/accept-extension")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails),
                        StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> AcceptExtension(Guid loanId)
  {
    var userIdResult = GetUserId();
    if (userIdResult.IsError)
      return Problem(userIdResult.Errors);

    var command = new AcceptLoanExtensionCommand(loanId, userIdResult.Value);
    var result = await _sender.Send(command);

    return result.Match(message => Ok(new { Message = message }), Problem);
  }

  [HttpPost("{loanId:guid}/request-extension")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
  [ProducesResponseType(typeof(ProblemDetails),
                        StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult>
  RequestExtension(Guid loanId, [FromBody] RequestExtensionRequest request)
  {
    var userIdResult = GetUserId();
    if (userIdResult.IsError)
      return Problem(userIdResult.Errors);

    var command = new RequestLoanExtensionCommand(loanId, userIdResult.Value,
                                                  request.ExtensionInDays);
    var result = await _sender.Send(command);

    return result.Match(message => Ok(new { Message = message }), Problem);
  }

  private ErrorOr<Guid> GetUserId()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Error.Unauthorized("User.Unauthorized",
                                "Invalid or missing user ID in token.");
    }
    return userId;
  }
}
