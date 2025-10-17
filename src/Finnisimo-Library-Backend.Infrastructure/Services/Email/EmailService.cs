using System.Net;
using System.Net.Mail;
using Finnisimo_Library_Backend.Application.Services.Email;
using Microsoft.Extensions.Options;

namespace Finnisimo_Library_Backend.Infrastructure.Services.Email;

internal sealed class EmailService(IOptions<EmailSettings> emailSettings)
    : IEmailService
{
  private readonly EmailSettings _emailSettings = emailSettings.Value;

  public async Task SendAsync(string recipient, string subject, string body)
  {
    var mailMessage = new MailMessage
    {
      From = new MailAddress(_emailSettings.From!),
      Subject = subject,
      Body = body,
      IsBodyHtml = true,
    };
    mailMessage.To.Add(recipient);

    var smtpClient = new SmtpClient(_emailSettings.SmtpServer!)
    {
      Port = _emailSettings.Port,
      Credentials = new NetworkCredential(_emailSettings.Username,
                                          _emailSettings.Password),
      EnableSsl = true,
    };

    await smtpClient.SendMailAsync(mailMessage);
  }
}
