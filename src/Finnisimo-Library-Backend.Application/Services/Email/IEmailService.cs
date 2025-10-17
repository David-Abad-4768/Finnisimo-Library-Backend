namespace Finnisimo_Library_Backend.Application.Services.Email;

public interface IEmailService
{
  Task SendAsync(string recipient, string subject, string body);
}
