using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Finnisimo_Library_Backend.WebApi.Gateways;

public class SignalRNotificationGateway(IHubContext<NotificationHub> hubContext)
    : INotificationGateway
{
  public async Task SendLoanReadyNotificationAsync(
      Guid userId, object payload,
      CancellationToken cancellationToken = default)
  {
    await hubContext.Clients.User(userId.ToString())
        .SendAsync("ReceiveLoanNotification", payload, cancellationToken);
  }

  public async Task
  SendNotificationAsync(Guid userId, string method, object payload,
                        CancellationToken cancellationToken = default)
  {
    await hubContext.Clients.User(userId.ToString())
        .SendAsync(method, payload, cancellationToken);
  }
}
