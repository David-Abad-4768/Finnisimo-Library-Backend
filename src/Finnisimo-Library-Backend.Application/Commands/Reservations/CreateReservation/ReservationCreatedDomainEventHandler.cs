using MediatR;
using Microsoft.Extensions.Logging;
using Finnisimo_Library_Backend.Application.Services.Email;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations.CreateReservation;

public class ReservationCreatedDomainEventHandler(
    IUserRepository userRepository,
    IBookRepository bookRepository,
    IEmailService emailService,
    ILogger<ReservationCreatedDomainEventHandler> logger)
    : INotificationHandler<ReservationCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<ReservationCreatedDomainEventHandler> _logger = logger;

    public async Task Handle(ReservationCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing ReservationCreatedDomainEvent for ReservationId: {ReservationId} (Email only)", domainEvent.ReservationId);

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
            _logger.LogInformation("Reservation confirmation email sent successfully to {UserEmail}", user.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to send reservation confirmation email for ReservationId: {ReservationId}", domainEvent.ReservationId);
        }
    }

    private async Task SendConfirmationEmailAsync(string userEmail, string userName, string bookTitle)
    {
        var subject = "Tu reservación ha sido confirmada";
        var body =
            $$"""
            <!DOCTYPE html>
            <html lang="es">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Reservación Confirmada</title>
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
                        <p>Hemos confirmado tu reservación para el libro:</p>
                        <p class="book-title">"{{bookTitle}}"</p>
                        <p>Serás el siguiente en la fila. Te enviaremos una notificación tan pronto como el libro esté disponible para que puedas solicitar el préstamo.</p>
                        <a href="https://finnisimo-library.com/my-reservations" class="cta-button">Ver mis Reservaciones</a>
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