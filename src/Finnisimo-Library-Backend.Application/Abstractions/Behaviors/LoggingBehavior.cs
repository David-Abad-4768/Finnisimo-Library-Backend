using MediatR;
using Microsoft.Extensions.Logging;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Commands;

namespace Finnisimo_Library_Backend.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand, IRequest<TResponse>
{
  private readonly ILogger<TRequest> _logger = logger;

  public async Task<TResponse> Handle(TRequest request,
                                      RequestHandlerDelegate<TResponse> next,
                                      CancellationToken cancellationToken)
  {
    var name = typeof(TRequest).Name;
    try
    {
      _logger.LogInformation("Executing command {CommandName}", name);

      var response = await next();

      _logger.LogInformation("Command {CommandName} executed successfully",
                             name);

      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Command {CommandName} encountered an error", name);
      throw;
    }
  }
}
