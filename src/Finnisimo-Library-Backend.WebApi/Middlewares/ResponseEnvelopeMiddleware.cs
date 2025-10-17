using System.Text.Json;

namespace Finnisimo_Library_Backend.WebApi.Middlewares;

public sealed class ResponseEnvelopeMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;
  private static readonly JsonSerializerOptions JsonOpts =
      new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
  private static readonly PathString[] Bypass = { "/swagger", "/favicon",
                                                  "/assets", "/health" };

  public async Task InvokeAsync(HttpContext context)
  {
    if (Bypass.Any(p => context.Request.Path.StartsWithSegments(p)))
    {
      await _next(context);
      return;
    }

    var originalBody = context.Response.Body;
    await using var mem = new MemoryStream();
    context.Response.Body = mem;

    try
    {
      await _next(context);

      mem.Seek(0, SeekOrigin.Begin);
      var raw = await new StreamReader(mem).ReadToEndAsync();
      mem.Seek(0, SeekOrigin.Begin);

      var contentType = context.Response.ContentType ?? "";
      var looksJson =
          raw.TrimStart().StartsWith("{") || raw.TrimStart().StartsWith("[");
      var isJson =
          contentType.Contains("json", StringComparison.OrdinalIgnoreCase) ||
          looksJson;
      if (!isJson)
      {
        context.Response.Body = originalBody;
        await mem.CopyToAsync(originalBody);
        return;
      }

      object? parsed;
      try
      {
        parsed = JsonSerializer.Deserialize<object>(raw, JsonOpts);
      }
      catch
      {
        parsed = raw;
      }

      var isError = context.Response.StatusCode >= 400;

      object envelope =
          isError ? new { error = true, data = new { }, errors = parsed }
                  : new { error = false, data = parsed ?? new { } };

      var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(envelope, JsonOpts);
      context.Response.Body = originalBody;
      context.Response.ContentType = "application/json; charset=utf-8";
      context.Response.ContentLength = jsonBytes.Length;
      await context.Response.Body.WriteAsync(jsonBytes, 0, jsonBytes.Length);
    }
    finally
    {
      if (context.Response.Body != originalBody)
        context.Response.Body = originalBody;
    }
  }
}
