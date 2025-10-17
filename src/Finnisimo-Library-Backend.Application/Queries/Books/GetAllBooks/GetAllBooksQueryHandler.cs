using AutoMapper;
using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Books.GetAllBooks;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

internal sealed class GetAllBooksQueryHandler(IBookRepository bookRepository,
                                              IMapper mapper)
    : IQueryHandler<GetAllBooksQuery, PaginedResponse<BookResponse>>
{
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IMapper _mapper = mapper;

  public async Task<ErrorOr<PaginedResponse<BookResponse>>>
  Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
  {
    PaginedResponse<BookEntity> booksFromDb =
        await _bookRepository.GetAllBooksAsync(
            request.Page, request.PageSize, request.Title, request.Author,
            request.PublicationYear, request.Genre, request.SortColumn,
            request.SortOrder, cancellationToken);

    var mappedItems =
        _mapper.Map<IReadOnlyList<BookResponse>>(booksFromDb.Items);

    var applicationResponse = new PaginedResponse<BookResponse>
    {
      Items = mappedItems,
      TotalCount = booksFromDb.TotalCount,
      Page = booksFromDb.Page,
      PageSize = booksFromDb.PageSize
    };

    return applicationResponse;
  }
}
