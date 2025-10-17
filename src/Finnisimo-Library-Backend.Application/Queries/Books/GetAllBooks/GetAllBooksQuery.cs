using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetAllBooks;

public sealed record GetAllBooksQuery(int Page, int PageSize, string? Title,
                                      string? Author, int? PublicationYear,
                                      string? Genre, string? SortColumn,
                                      string? SortOrder)
    : IQuery<PaginedResponse<BookResponse>>;
