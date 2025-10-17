using AutoMapper;
using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Loans.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserLoans;

internal sealed class GetUserLoansQueryHandler(ILoanRepository loanRepository,
                                               IMapper mapper)
    : IQueryHandler<GetUserLoansQuery, PaginedResponse<UserLoanResponse>>
{
  public async Task<ErrorOr<PaginedResponse<UserLoanResponse>>>
  Handle(GetUserLoansQuery request, CancellationToken cancellationToken)
  {
    PaginedResponse<LoanEntity> loansFromDb =
        await loanRepository.GetLoansWithBookDetailsByUserIdAsync(
            request.UserId, request.Page, request.PageSize, request.Status,
            cancellationToken);

    var mappedItems = mapper.Map<List<UserLoanResponse>>(loansFromDb.Items);

    foreach (var loan in mappedItems)
    {
      var timeSpan = loan.DueDate - DateTime.UtcNow;

      loan.TimeRemaining = timeSpan.TotalSeconds > 0 ? timeSpan : TimeSpan.Zero;

      loan.TimeRemainingFriendly = FormatTimeRemaining(timeSpan);
    }

    var applicationResponse = new PaginedResponse<UserLoanResponse>
    {
      Items = mappedItems,
      TotalCount = loansFromDb.TotalCount,
      Page = loansFromDb.Page,
      PageSize = loansFromDb.PageSize
    };

    return applicationResponse;
  }

  private static string FormatTimeRemaining(TimeSpan timeSpan)
  {
    if (timeSpan.TotalSeconds <= 0)
    {
      return "Overdue";
    }

    if (timeSpan.Days > 0)
    {
      return $"Due in {timeSpan.Days} day(s) and {timeSpan.Hours} hour(s)";
    }

    if (timeSpan.Hours > 0)
    {
      return $"Due in {timeSpan.Hours} hour(s) and {timeSpan.Minutes} minute(s)";
    }

    return $"Due in {timeSpan.Minutes} minute(s)";
  }
}
