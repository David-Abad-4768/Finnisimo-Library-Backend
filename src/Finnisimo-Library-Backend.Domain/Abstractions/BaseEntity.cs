using System.Collections.Generic;
using System.Linq;

namespace Finnisimo_Library_Backend.Domain.Abstractions;

public abstract class BaseEntity(Guid id)
{
  private readonly List<IDomainEvent> _domainEvents = [];

  public Guid Id { get; init; } = id;
  public IReadOnlyList<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

  public void ClearDomainEvents() => _domainEvents.Clear();

  protected void RaiseDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents.Add(domainEvent);
  }
}
