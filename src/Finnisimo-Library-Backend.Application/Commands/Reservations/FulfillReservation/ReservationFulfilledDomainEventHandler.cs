using MediatR;
using Microsoft.Extensions.Configuration;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;
using Finnisimo_Library_Backend.Domain.Entities.Reservations.Events;

namespace Finnisimo_Library_Backend.Application.Commands.Reservations
    .FulfillReservation;

public class ReservationFulfilledDomainEventHandler(
    ILoanRepository loanRepository, IUnitOfWork unitOfWork,
    IConfiguration configuration)
    : INotificationHandler<ReservationFulfilledDomainEvent>
{
  private readonly ILoanRepository _loanRepository = loanRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IConfiguration _configuration = configuration;

  public async Task Handle(ReservationFulfilledDomainEvent notification,
                           CancellationToken cancellationToken)
  {
    var defaultLoanDays =
        _configuration.GetValue("LibrarySettings:DefaultLoanDurationDays", 14);
    var loanDuration = TimeSpan.FromDays(defaultLoanDays);

    var newLoan = LoanEntity.Create(notification.BookId, notification.UserId);

    newLoan.Approve(DateTime.UtcNow, loanDuration);

    await _loanRepository.AddAsync(newLoan, cancellationToken);

    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
