using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;

namespace Finnisimo_Library_Backend.Application.Queries.Statistics
    .GetPagesReadPerBook;

public sealed record GetPagesReadPerBookQuery(Guid UserId, DateTime TargetDate,
                                              TimePeriod Period)
    : IQuery<IEnumerable<BookPagesReadResponse>>;
