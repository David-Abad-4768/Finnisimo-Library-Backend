using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserLoans;

public sealed record GetUserLoansQuery(Guid UserId, int Page, int PageSize,
                                       LoanStatus? Status)
    : IQuery<PaginedResponse<UserLoanResponse>>;
