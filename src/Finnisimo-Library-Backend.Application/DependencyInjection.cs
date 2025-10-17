using FluentValidation;
using Finnisimo_Library_Backend.Application.Abstractions.Behaviors;
using Finnisimo_Library_Backend.Application.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Finnisimo_Library_Backend.Application;

public static class DependencyInjection
{
  public static IServiceCollection
  AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(configuration =>
    {
      configuration.RegisterServicesFromAssembly(
          ApplicationAssemblyReference.Assembly);

      configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
      configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);
    services.AddAutoMapper(typeof(MappingProfile).Assembly);

    return services;
  }
}
