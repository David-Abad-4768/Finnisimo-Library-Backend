using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent { }
