using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetAllAuthors;

internal sealed class GetAllAuthorsQueryHandler(IBookRepository bookRepository)
    : IQueryHandler<GetAllAuthorsQuery, IReadOnlyList<string>>
{
  private readonly IBookRepository _bookRepository = bookRepository;

  public async Task<ErrorOr<IReadOnlyList<string>>>
  Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
  {
    var authors = await _bookRepository.GetAllAuthorsAsync(cancellationToken);
    return authors.ToList();
  }
}
