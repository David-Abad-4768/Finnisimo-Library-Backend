namespace Finnisimo_Library_Backend.WebApi.Extensions;

public static class CorsExtensions
{
  public const string SingleOriginPolicy = "SingleOriginPolicy";

  public static IServiceCollection
  AddConfiguredCors(this IServiceCollection services,
                    IConfiguration configuration)
  {
    var origins =
        configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    var single = configuration["Cors:Origin"];
    if (origins.Length == 0 && !string.IsNullOrWhiteSpace(single))
    {
      origins = [single!];
    }

    if (origins.Length == 0)
    {
      throw new InvalidOperationException(
          "No CORS origins configured. Set Cors:AllowedOrigins (array) or " +
          "Cors:Origin (string).");
    }

    services.AddCors(options =>
    {
      options.AddPolicy(SingleOriginPolicy, builder =>
      {
        if (Array.Exists(origins, o => o == "*"))
        {
          builder
              .SetIsOriginAllowed(
                  _ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        }
        else
        {
          builder.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        }
      });
    });

    return services;
  }

  public static IApplicationBuilder UseConfiguredCors(
      this IApplicationBuilder app) => app.UseCors(SingleOriginPolicy);
}
