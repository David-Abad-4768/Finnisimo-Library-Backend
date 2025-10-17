using System.Text.Json;
using Finnisimo_Library_Backend.Application.Exceptions;

namespace Finnisimo_Library_Backend.WebApi.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
  private readonly RequestDelegate _next = next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
  private static readonly JsonSerializerOptions JsonOpts =
      new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

      (int status, IEnumerable<object> errors) = ex switch
      {
        ValidationException ve =>
            (StatusCodes.Status400BadRequest,
             ve.Errors.GroupBy(e => e.PropertyName)
                 .Select(g => new
                 {
                   field = g.Key,
                   messages = g.Select(x => x.ErrorMessage).Distinct().ToArray()
                 })
                 .Cast<object>()),
        _ => (
            StatusCodes.Status500InternalServerError,
            new[] { new { message = "Internal server error." } }.Cast<object>())
      };

      context.Response.StatusCode = status;
      context.Response.ContentType = "application/json; charset=utf-8";

      var payload = new
      {
        error = true,
        data = new { },
        errors,
        traceId = context.TraceIdentifier
      };

      await context.Response.WriteAsync(
          JsonSerializer.Serialize(payload, JsonOpts));
    }
  }
}
