using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetBookById;

public sealed record GetBookByIdQuery(Guid BookId) : IQuery<BookDetailResponse>;
