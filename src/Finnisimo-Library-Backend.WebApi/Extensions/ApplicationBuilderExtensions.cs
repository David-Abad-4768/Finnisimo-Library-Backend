using Finnisimo_Library_Backend.Infrastructure.Persistence;
using Finnisimo_Library_Backend.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Finnisimo_Library_Backend.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder
  ApplyMigrations(this IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(
        "ApplyMigrations");
    try
    {
      var context = services.GetRequiredService<ApplicationDbContext>();
      context.Database.Migrate();
      logger.LogInformation("Database migrated successfully.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while migrating the database.");
    }
    return app;
  }
  public static void UseCustomExceptionHandler(this IApplicationBuilder app)
  {
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<ResponseEnvelopeMiddleware>();
  }
}
