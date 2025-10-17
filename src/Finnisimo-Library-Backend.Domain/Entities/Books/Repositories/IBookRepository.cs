using Finnisimo_Library_Backend.Domain.Abstractions;

namespace Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;

public interface IBookRepository : IBaseRepository<BookEntity, Guid>
{
  Task<bool> DoesBookExistAsync(string title, string author,
                                CancellationToken cancellationToken = default);

  Task<PaginedResponse<BookEntity>>
  GetAllBooksAsync(int page, int pageSize, string? title, string? author,
                   int? publicationYear, string? genre, string? sortColumn,
                   string? sortOrder,
                   CancellationToken cancellationToken = default);

  Task<IReadOnlyList<string>>
  GetAllGenresAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyList<string>>
  GetAllAuthorsAsync(CancellationToken cancellationToken = default);
}
