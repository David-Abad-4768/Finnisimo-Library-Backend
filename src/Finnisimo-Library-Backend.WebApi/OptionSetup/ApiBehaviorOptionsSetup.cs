using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Finnisimo_Library_Backend.WebApi.OptionSetup;

public class ApiBehaviorOptionsSetup : IConfigureOptions<ApiBehaviorOptions>
{
  public void Configure(ApiBehaviorOptions options)
  {
    options.InvalidModelStateResponseFactory = context =>
    {
      var errors =
          context.ModelState.Where(kvp => kvp.Value?.Errors.Count > 0)
              .Select(kvp => new
              {
                field = kvp.Key,
                messages =
                    kvp.Value!.Errors
                        .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                                         ? "Invalid value."
                                         : e.ErrorMessage)
                        .Distinct()
                        .ToArray()
              })
              .ToArray();

      var payload = new { error = true, data = new { }, errors };

      return new JsonResult(payload)
      {
        StatusCode =
                                           StatusCodes.Status400BadRequest
      };
    };
  }
}
