using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetLastProgress;

public sealed record GetLastProgressQuery(Guid UserId, Guid LoanId)
    : IQuery<LastProgressResponse>;
