using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Events;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;
using Finnisimo_Library_Backend.Domain.Entities.Users;

namespace Finnisimo_Library_Backend.Domain.Entities.Loans;

public sealed class LoanEntity : BaseEntity
{
  private LoanEntity(Guid id, Guid bookId, Guid userId) : base(id)
  {
    BookId = bookId;
    UserId = userId;
    Status = LoanStatus.Requested;
    RequestedAt = DateTime.UtcNow;
    LoanedAt = null;
    DueDate = null;
    ReturnedAt = null;
  }

  public Guid BookId { get; private set; }
  public Guid UserId { get; private set; }
  public LoanStatus Status { get; private set; }
  public DateTime RequestedAt { get; private set; }
  public DateTime? LoanedAt { get; private set; }
  public DateTime? DueDate { get; private set; }
  public DateTime? ReturnedAt { get; private set; }
  public DateTime? ExtensionOfferedAt { get; private set; }
  public DateTime? ExtensionOfferExpiresAt { get; private set; }

  public static LoanEntity Create(Guid bookId, Guid userId)
  {
    var loan = new LoanEntity(Guid.NewGuid(), bookId, userId);

    loan.RaiseDomainEvent(
        new LoanCreatedDomainEvent(loan.Id, loan.UserId, loan.BookId));

    return new LoanEntity(Guid.NewGuid(), bookId, userId);
  }

  public void Approve(DateTime approvalDate, TimeSpan loanDuration)
  {
    if (Status != LoanStatus.Requested)
    {
      throw new InvalidOperationException(
          "Only a requested loan can be approved.");
    }
    Status = LoanStatus.Approved;
    LoanedAt = approvalDate;
    DueDate = approvalDate.Add(loanDuration);
  }

  public void Reject()
  {
    if (Status != LoanStatus.Requested)
    {
      throw new InvalidOperationException(
          "Only a requested loan can be rejected.");
    }
    Status = LoanStatus.Rejected;
  }

  public void Cancel()
  {
    if (Status != LoanStatus.Approved && Status != LoanStatus.Requested)
    {
      throw new InvalidOperationException(
          "Only an approved or requested loan can be cancelled.");
    }
    Status = LoanStatus.Cancelled;
  }

  public void MarkAsReturned(DateTime returnDate)
  {
    if (Status != LoanStatus.Approved && Status != LoanStatus.Overdue)
    {
      throw new InvalidOperationException(
          "Cannot return a loan that is not currently active.");
    }
    Status = LoanStatus.Returned;
    ReturnedAt = returnDate;

    RaiseDomainEvent(new LoanReturnedDomainEvent(BookId, UserId));
  }

  public void MarkAsOverdue()
  {
    if (Status != LoanStatus.Approved)
    {
      throw new InvalidOperationException(
          "Only an approved loan can be marked as overdue.");
    }
    Status = LoanStatus.Overdue;
  }

  public void OfferExtension()
  {
    if (Status != LoanStatus.Approved)
    {
      throw new InvalidOperationException(
          "An extension can only be offered for an approved loan.");
    }
    ExtensionOfferedAt = DateTime.UtcNow;
    ExtensionOfferExpiresAt = DateTime.UtcNow.AddHours(24);
  }

  public void AcceptExtension(TimeSpan extensionDuration)
  {
    if (ExtensionOfferExpiresAt is null ||
        ExtensionOfferExpiresAt.Value < DateTime.UtcNow)
    {
      throw new InvalidOperationException(
          "There is no valid extension offer to accept.");
    }

    if (DueDate is null)
    {
      throw new InvalidOperationException(
          "Cannot extend a loan without a due date.");
    }

    DueDate = DueDate.Value.Add(extensionDuration);

    ExtensionOfferedAt = null;
    ExtensionOfferExpiresAt = null;
  }

  public void Extend(TimeSpan extensionDuration)
  {
    if (Status != LoanStatus.Approved)
    {
      throw new InvalidOperationException(
          "Only an approved loan can be extended.");
    }

    if (DueDate is null)
    {
      throw new InvalidOperationException(
          "Cannot extend a loan that does not have a due date.");
    }

    DueDate = DueDate.Value.Add(extensionDuration);
  }

  public BookEntity Book { get; private set; } = null!;
  public UserEntity User { get; private set; } = null!;
  public ICollection<ReadingLogEntryEntity> ReadingLog
  {
    get; private set;
  } = [];
}
