using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

public interface ILoanRepository : IBaseRepository<LoanEntity, Guid>
{
  Task<bool> IsBookOnLoanAsync(Guid bookId,
                               CancellationToken cancellationToken = default);

  Task<bool> HasPendingOrActiveLoanForBookAsync(
      Guid userId, Guid bookId, CancellationToken cancellationToken = default);

  Task<PaginedResponse<LoanEntity>> GetLoansWithBookDetailsByUserIdAsync(
      Guid userId, int page, int pageSize, LoanStatus? status,
      CancellationToken cancellationToken = default);

  Task<IReadOnlyList<LoanEntity>>
  GetOverdueLoansAsync(CancellationToken cancellationToken = default);

  Task<IReadOnlyList<LoanEntity>>
  GetLoansDueInRangeAsync(DateTime from, DateTime to,
                          CancellationToken cancellationToken = default);
}
