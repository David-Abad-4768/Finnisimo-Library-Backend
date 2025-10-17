using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finnisimo_Library_Backend.WebApi.Controllers.v1.User.Request;
using Finnisimo_Library_Backend.Application.Commands.Users.LoginUser;
using Finnisimo_Library_Backend.Application.Commands.Users.CreateUser;
using Finnisimo_Library_Backend.Application.Commands.Users.DeleteUser;
using Finnisimo_Library_Backend.Application.Commands.Users.UpdateUser;
using Finnisimo_Library_Backend.Application.Queries.Users.GetUserById;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Application.Queries.Users.GetUserReservations;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Application.Queries.Users.GetUserLoans;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.User;

[ApiController]
[Route("api/v1/user")]
public class UserController(IMediator mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult>
  CreateUser([FromBody] CreateUserRequest request)
  {
    var command = new CreateUserCommand(request.Name, request.LastName,
                                        request.Email, request.Username,
                                        request.Password, request.DateOfBirth);

    var result = await _mediator.Send(command);

    return result.Match(message => StatusCode(201, new { message }), Problem);
  }

  [HttpPost("login")]
  public async Task<IActionResult>
  LoginUser([FromBody] LoginUserRequest request)
  {
    var command = new LoginUserCommand(request.Username, request.Password);
    var result = await _mediator.Send(command);

    return result.Match(loginResponse =>
    {
      Response.Cookies.Append(
          "access_token", loginResponse.AccessToken,
          new CookieOptions
          {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTime.UtcNow.AddMinutes(1440)
          });

      return Ok(new { message = loginResponse.Message });
    }, Problem);
  }

  [Authorize]
  [HttpPost("logout")]
  public IActionResult LogoutUser()
  {
    var jwt = User.FindFirstValue(
        System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti);

    if (jwt is not null)
    {
      _ = $"session:{jwt}";
    }

    Response.Cookies.Delete("access_token");

    return Ok(new { message = "Logout successful" });
  }

  [Authorize]
  [HttpGet()]
  public async Task<IActionResult> GetUserById()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var result = await _mediator.Send(new GetUserByIdQuery(userId));

    return result.Match(Ok, Problem);
  }

  [Authorize]
  [HttpPut]
  public async Task<IActionResult>
  UpdateUser([FromBody] UpdateUserRequest request)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var command = new UpdateUserCommand(userId, request.Name, request.LastName,
                                        request.Email, request.Username);

    var result = await _mediator.Send(command);

    return result.Match(
        _ => Ok(new { message = "User updated successfully" }), Problem);
  }

  [Authorize]
  [HttpDelete()]
  public async Task<IActionResult> DeleteUser()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
      return Unauthorized("Invalid or missing user ID in token.");

    var result = await _mediator.Send(new DeleteUserCommand(userId));

    return result.Match(Ok, Problem);
  }

  [Authorize]
  [HttpGet("check-auth")]
  public IActionResult CheckAuth()
  {
    return Ok(new { message = "Token is present and valid" });
  }

  [Authorize]
  [HttpGet("reservations")]
  public async Task<IActionResult>
  GetUserReservations([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
                      [FromQuery] ReservationStatus? status = null)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var query = new GetUserReservationsQuery(userId, page, pageSize, status);

    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [Authorize]
  [HttpGet("loans")]
  public async Task<IActionResult>
  GetUserLoans([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
               [FromQuery] LoanStatus? status = null)
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

    if (userIdClaim is null ||
        !Guid.TryParse(userIdClaim.Value, out var userId))
    {
      return Unauthorized("Invalid or missing user ID in token.");
    }

    var query = new GetUserLoansQuery(userId, page, pageSize, status);

    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }
}
