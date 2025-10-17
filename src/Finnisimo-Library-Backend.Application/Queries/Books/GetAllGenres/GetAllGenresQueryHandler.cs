using ErrorOr;
using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetAllGenres;

internal sealed class GetAllGenresQueryHandler(IBookRepository bookRepository)
    : IQueryHandler<GetAllGenresQuery, IReadOnlyList<string>>
{
  private readonly IBookRepository _bookRepository = bookRepository;

  public async Task<ErrorOr<IReadOnlyList<string>>>
  Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
  {
    var genres = await _bookRepository.GetAllGenresAsync(cancellationToken);

    return genres.ToList();
  }
}
