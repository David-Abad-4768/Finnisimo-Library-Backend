using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Finnisimo_Library_Backend.WebApi.Middlewares;

public class SessionValidationMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context)
  {
    if (context.User.Identity?.IsAuthenticated == true)
    {
      var jwt = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

      if (jwt is null)
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
      }
    }

    await _next(context);
  }
}
