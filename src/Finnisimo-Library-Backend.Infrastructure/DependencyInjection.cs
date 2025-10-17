using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Application.Services.Clock;
using Finnisimo_Library_Backend.Application.Services.Redis;
using Finnisimo_Library_Backend.Application.Services.Email;
using Finnisimo_Library_Backend.Infrastructure.Persistence;
using Finnisimo_Library_Backend.Infrastructure.Services.Redis;
using Finnisimo_Library_Backend.Infrastructure.Services.Clock;
using Finnisimo_Library_Backend.Infrastructure.Services.Email;
using Finnisimo_Library_Backend.Domain.Entities.Users.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Finnisimo_Library_Backend.Application.Services.HashedPassword;
using Finnisimo_Library_Backend.Infrastructure.Services.HashedPassword;
using Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Notifications.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem.Repositories;

namespace Finnisimo_Library_Backend.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection
  AddInfrastructure(this IServiceCollection services,
                    IConfiguration configuration)
  {

    var redisConnectionString =
        configuration.GetConnectionString("Redis:ConnectionString") ??
        "localhost:6379";

    if (!string.IsNullOrEmpty(redisConnectionString))
    {
      services.AddSingleton<IConnectionMultiplexer>(
          ConnectionMultiplexer.Connect(redisConnectionString));

      services.AddTransient<IRedisService, RedisService>();
    }

    services.Configure<EmailSettings>(
        configuration.GetSection("EmailSettings"));

    services.AddTransient<IDateTimeService, DateTimeService>();
    services.AddTransient<IEmailService, EmailService>();
    services.AddTransient<IHashedPasswordService, HashedPasswordService>();

    var connectionString =
        configuration.GetConnectionString("Database") ??
        throw new ArgumentNullException(nameof(configuration));
    services.AddDbContext<ApplicationDbContext>(
        options => options.UseNpgsql(connectionString));

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ILoanRepository, LoanRepository>();
    services.AddScoped<IReservationRepository, ReservationRepository>();
    services.AddScoped<IBookRepository, BookRepository>();
    services.AddScoped<IReadingLogRepository, ReadingLogRepository>();
    services.AddScoped<INotificationRepository, NotificationRepository>();
    services.AddScoped<IWishlistItemRepository, WishlistItemRepository>();

    services.AddScoped<IUnitOfWork>(
        sp => sp.GetRequiredService<ApplicationDbContext>());
    return services;
  }
}
