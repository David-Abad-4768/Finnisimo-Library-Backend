namespace Finnisimo_Library_Backend.Application.Abstractions.Gateways;

public interface INotificationGateway
{
  Task
  SendLoanReadyNotificationAsync(Guid userId, object payload,
                                 CancellationToken cancellationToken = default);

  Task SendNotificationAsync(Guid userId, string method, object payload,
                             CancellationToken cancellationToken = default);
}
