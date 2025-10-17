using AutoMapper;
using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetBookById;

internal sealed class GetBookByIdQueryHandler(IBookRepository bookRepository,
                                              IMapper mapper)
    : IQueryHandler<GetBookByIdQuery, BookDetailResponse>
{
  private readonly IBookRepository _bookRepository = bookRepository;
  private readonly IMapper _mapper = mapper;

  public async Task<ErrorOr<BookDetailResponse>>
  Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
  {
    var book =
        await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);

    if (book is null)
    {
      return Error.NotFound(
          code: "Book.NotFound",
          description: $"The book with ID '{request.BookId}' was not found.");
    }

    var response = _mapper.Map<BookDetailResponse>(book);

    return response;
  }
}
