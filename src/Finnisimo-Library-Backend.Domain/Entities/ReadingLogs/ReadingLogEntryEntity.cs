using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;

namespace Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;

public sealed class ReadingLogEntryEntity : BaseEntity
{
  public Guid LoanId { get; private set; }
  public Guid UserId { get; private set; }
  public int StartPage { get; private set; }
  public int EndPage { get; private set; }
  public int PagesRead { get; private set; }
  public DateTime DateLogged { get; private set; }

  public LoanEntity Loan { get; private set; } = null!;

  private ReadingLogEntryEntity(Guid id, Guid loanId, Guid userId,
                                int startPage, int endPage)
      : base(id)
  {
    if (endPage < startPage)
    {
      throw new ArgumentException("End page cannot be less than start page.");
    }

    LoanId = loanId;
    UserId = userId;
    StartPage = startPage;
    EndPage = endPage;
    PagesRead = endPage - startPage;
    DateLogged = DateTime.UtcNow;
  }

  public static ReadingLogEntryEntity Create(Guid loanId, Guid userId,
                                             int startPage, int endPage)
  {
    return new ReadingLogEntryEntity(Guid.NewGuid(), loanId, userId, startPage,
                                     endPage);
  }
}
