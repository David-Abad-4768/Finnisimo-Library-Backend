using MediatR;
using Microsoft.Extensions.Logging;
using Finnisimo_Library_Backend.Application.Services.Email;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Events;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Loans.CreateLoan;

public class LoanCreatedDomainEventHandler(
    IUserRepository userRepository,
    IBookRepository bookRepository,
    IEmailService emailService,
    ILogger<LoanCreatedDomainEventHandler> logger)
    : INotificationHandler<LoanCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<LoanCreatedDomainEventHandler> _logger = logger;

    public async Task Handle(LoanCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing LoanCreatedDomainEvent for LoanId: {LoanId} (Email only)", domainEvent.LoanId);

        try
        {
            var user = await _userRepository.GetByIdAsync(domainEvent.UserId, cancellationToken);
            var book = await _bookRepository.GetByIdAsync(domainEvent.BookId, cancellationToken);

            if (user is null || book is null)
            {
                _logger.LogError("Failed to process event: User or book not found. Cannot send email.");
                return;
            }

            await SendConfirmationEmailAsync(user.Email, user.Name, book.Title);
            _logger.LogInformation("Loan request confirmation email sent successfully to {UserEmail}", user.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to send loan confirmation email for LoanId: {LoanId}", domainEvent.LoanId);
        }
    }

    private async Task SendConfirmationEmailAsync(string userEmail, string userName, string bookTitle)
    {
        var subject = "Confirmación de tu solicitud de préstamo";
        var body =
            $$"""
            <!DOCTYPE html>
            <html lang="es">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Solicitud de Préstamo Recibida</title>
                <style>
                    body { font-family: 'Georgia', serif; margin: 0; padding: 0; background-color: #fdfaf6; }
                    .container { max-width: 600px; margin: 20px auto; background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.08); }
                    .header { background-color: #4a3728; color: #ffffff; padding: 40px; text-align: center; }
                    .header h1 { margin: 0; font-size: 24px; font-weight: normal; }
                    .content { padding: 30px; line-height: 1.7; color: #555555; }
                    .content h2 { color: #4a3728; }
                    .book-title { font-style: italic; color: #333; }
                    .cta-button { display: inline-block; margin-top: 20px; padding: 12px 25px; background-color: #8a6d4a; color: #ffffff; text-decoration: none; border-radius: 5px; }
                    .footer { background-color: #f8f8f8; color: #888; padding: 20px; text-align: center; font-size: 12px; border-top: 1px solid #e0e0e0; }
                </style>
            </head>
            <body>
                <div class="container">
                    <div class="header">
                        <h1>BIBLIOTECA FINNISSIMO</h1>
                    </div>
                    <div class="content">
                        <h2>Hola, {{userName}}!</h2>
                        <p>Hemos recibido correctamente tu solicitud de préstamo para el libro:</p>
                        <p class="book-title">"{{bookTitle}}"</p>
                        <p>Tu solicitud está siendo procesada por nuestros bibliotecarios. Recibirás otra notificación tan pronto como sea aprobada.</p>
                        <a href="https://finnisimo-library.com/my-loans" class="cta-button">Ver mis Préstamos</a>
                    </div>
                    <div class="footer">
                        <p>&copy; 2025 Biblioteca Finnisimo. Todos los derechos reservados.</p>
                    </div>
                </div>
            </body>
            </html>
            """;

        await _emailService.SendAsync(userEmail, subject, body);
    }
}