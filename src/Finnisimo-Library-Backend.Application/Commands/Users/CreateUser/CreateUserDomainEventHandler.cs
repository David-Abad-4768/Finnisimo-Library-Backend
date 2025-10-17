using MediatR;
using Finnisimo_Library_Backend.Application.Services.Email;
using Finnisimo_Library_Backend.Domain.Entities.Users.Events;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;

namespace Finnisimo_Library_Backend.Application.Commands.Users.CreateUser;

public class CreateUserDomainEventHandler(
    IUserRepository userRepository,
    IEmailService emailService)
    : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
        if (user is null)
        {
            return;
        }

        var subject = "¡Bienvenido a la Biblioteca Finnisimo!";

        var body =
            $$"""
            <!DOCTYPE html>
            <html lang="es">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Bienvenido a la Biblioteca Finnisimo</title>
                <style>
                    body { font-family: 'Georgia', serif; margin: 0; padding: 0; background-color: #fdfaf6; }
                    .container { max-width: 600px; margin: 20px auto; background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.08); }
                    .header { background-color: #4a3728; color: #ffffff; padding: 40px; text-align: center; }
                    .header h1 { margin: 0; font-size: 28px; font-weight: normal; letter-spacing: 2px; }
                    .content { padding: 30px; line-height: 1.7; color: #555555; }
                    .content h2 { color: #4a3728; }
                    .cta-button { display: block; width: 220px; margin: 30px auto; padding: 15px 20px; background-color: #8a6d4a; color: #ffffff; text-align: center; text-decoration: none; border-radius: 5px; font-weight: bold; }
                    .footer { background-color: #f8f8f8; color: #888; padding: 20px; text-align: center; font-size: 12px; border-top: 1px solid #e0e0e0; }
                    .footer a { color: #8a6d4a; text-decoration: none; }
                </style>
            </head>
            <body>
                <div class="container">
                    <div class="header">
                        <h1>BIBLIOTECA FINNISSIMO</h1>
                    </div>
                    <div class="content">
                        <h2>¡Bienvenido, {{user.Name}}!</h2>
                        <p>Tu cuenta ha sido creada exitosamente. Un universo de historias, conocimiento y aventura te espera.</p>
                        <p>Comienza a explorar nuestra vasta colección, descubre nuevos autores y encuentra tu próxima lectura favorita. Las puertas están abiertas.</p>
                        <a href="https://finnisimo-library.com/explore" class="cta-button">Explorar la Colección</a>
                    </div>
                    <div class="footer">
                        <p>&copy; 2025 Biblioteca Finnisimo. Todos los derechos reservados.</p>
                        <p>Si no te registraste en nuestra biblioteca, por favor ignora este correo.</p>
                    </div>
                </div>
            </body>
            </html>
            """;

        await _emailService.SendAsync(user.Email, subject, body);
    }
}
