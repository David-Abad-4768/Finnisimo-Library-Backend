using Finnisimo_Library_Backend.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
  private readonly IPublisher _publisher;
  private readonly ILogger<ApplicationDbContext> _logger;

  public ApplicationDbContext(DbContextOptions options, IPublisher publisher,
                              ILoggerFactory loggerFactory)
      : base(options)
  {
    _publisher = publisher;
    _logger = loggerFactory.CreateLogger<ApplicationDbContext>();

    _logger.LogWarning("--- NEW ApplicationDbContext INSTANCE CREATED --- " +
                           "HashCode: {HashCode}",
                       this.GetHashCode());
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(ApplicationDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }

  public override async Task<int>
  SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Llamando a SaveChangesAsync. Intentando guardar " +
                           "cambios en la base de datos.");
    var result = await base.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("Cambios guardados exitosamente. Intentando " +
                           "publicar eventos de dominio.");
    await PublishDomainEventsAsync();
    return result;
  }

  private async Task PublishDomainEventsAsync()
  {
    var entitiesWithEvents =
        ChangeTracker.Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.GetDomainEvents().Any())
            .ToList();

    if (!entitiesWithEvents.Any())
    {
      _logger.LogWarning("No se encontraron entidades con eventos de dominio " +
                         "en el ChangeTracker después de guardar.");
      return;
    }

    var domainEvents = entitiesWithEvents
                           .SelectMany(entity =>
                           {
                             var events = entity.GetDomainEvents();
                             entity.ClearDomainEvents();
                             return events;
                           })
                           .ToList();

    _logger.LogInformation("Publicando un total de {Count} eventos de dominio.",
                           domainEvents.Count);

    foreach (var domainEvent in domainEvents)
    {
      _logger.LogInformation(
          "--> Publicando evento de dominio: {DomainEventName}",
          domainEvent.GetType().Name);
      await _publisher.Publish(domainEvent);
      _logger.LogInformation(
          "--> Evento {DomainEventName} publicado exitosamente",
          domainEvent.GetType().Name);
    }
  }
}
