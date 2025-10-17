using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Loans.Events;

public sealed record LoanCreatedDomainEvent(Guid LoanId, Guid UserId,
                                            Guid BookId)
    : IDomainEvent;
