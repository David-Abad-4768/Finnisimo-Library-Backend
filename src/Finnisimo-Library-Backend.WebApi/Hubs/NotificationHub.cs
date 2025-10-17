using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Finnisimo_Library_Backend.WebApi.Hubs;

[Authorize]
public class NotificationHub : Hub { }
