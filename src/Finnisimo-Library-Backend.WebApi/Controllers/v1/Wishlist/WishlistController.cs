using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .CreateWishlistItem;
using Finnisimo_Library_Backend.Application.Commands.WishlistItem
    .DeleteWishlistItem;
using Finnisimo_Library_Backend.Application.Queries.Users.GetUserWishlist;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Wishlist;

[Authorize]
[ApiController]
[Route("api/v1/wishlist")]
public class WishlistController(IMediator mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpGet]
  public async Task<IActionResult>
  GetUserWishlist([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var query = new GetUserWishlistQuery(userId, page, pageSize);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpPost("{bookId:guid}")]
  public async Task<IActionResult> CreateWishlistItem([FromRoute] Guid bookId)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var command = new CreateWishlistItemCommand(userId, bookId);
    var result = await _mediator.Send(command);

    return result.Match(message => Ok(new { message }), Problem);
  }

  [HttpDelete("{bookId:guid}")]
  public async Task<IActionResult> DeleteWishlistItem([FromRoute] Guid bookId)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var command = new DeleteWishlistItemCommand(userId, bookId);
    var result = await _mediator.Send(command);

    return result.Match(
        _ => NoContent(), Problem);
  }
}
