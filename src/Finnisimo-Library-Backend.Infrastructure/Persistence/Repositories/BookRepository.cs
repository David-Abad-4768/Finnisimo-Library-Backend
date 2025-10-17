using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Books.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Repositories;

public sealed class BookRepository(ApplicationDbContext dbContext)
    : BaseRepository<BookEntity, Guid>(dbContext), IBookRepository
{
  private readonly ApplicationDbContext _db = dbContext;

  public async Task<bool>
  DoesBookExistAsync(string title, string author,
                     CancellationToken cancellationToken = default)
  {
    return await _db.Set<BookEntity>().AnyAsync(
        b => b.Title == title && b.Author == author, cancellationToken);
  }

  public async Task<PaginedResponse<BookEntity>>
  GetAllBooksAsync(int page, int pageSize, string? title, string? author,
                   int? publicationYear, string? genre, string? sortColumn,
                   string? sortOrder,
                   CancellationToken cancellationToken = default)
  {
    var query = _db.Set<BookEntity>().AsNoTracking();

    if (!string.IsNullOrWhiteSpace(title))
    {
      query = query.Where(b => b.Title.Contains(title));
    }

    if (!string.IsNullOrWhiteSpace(author))
    {
      query = query.Where(b => b.Author.Contains(author));
    }

    if (publicationYear.HasValue)
    {
      query = query.Where(b => b.PublicationDate.Year == publicationYear.Value);
    }

    if (!string.IsNullOrWhiteSpace(genre))
    {
      query = query.Where(b => b.Genre == genre);
    }

    query = (sortColumn?.ToLower(), sortOrder?.ToLower()) switch
    {
      ("title", "desc") => query.OrderByDescending(b => b.Title),
      ("title", _) => query.OrderBy(b => b.Title),
      ("author", "desc") => query.OrderByDescending(b => b.Author),
      ("author", _) => query.OrderBy(b => b.Author),
      ("publishedyear", "desc") =>
          query.OrderByDescending(b => b.PublicationDate),
      ("publishedyear", _) => query.OrderBy(b => b.PublicationDate),

      ("popularity", "desc") => query.OrderByDescending(b => b.TimesLoaned),
      ("popularity", _) => query.OrderBy(b => b.TimesLoaned),

      _ => query.OrderByDescending(b => b.TimesLoaned)
    };

    var total = await query.CountAsync(cancellationToken);

    var items = await query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

    return new PaginedResponse<BookEntity>
    {
      Items = items,
      TotalCount = total,
      Page = page,
      PageSize = pageSize
    };
  }

  public async Task<IReadOnlyList<string>>
  GetAllGenresAsync(CancellationToken cancellationToken = default)
  {
    return await _db.Set<BookEntity>()
        .Select(b => b.Genre)
        .Distinct()
        .OrderBy(g => g)
        .ToListAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<string>>
  GetAllAuthorsAsync(CancellationToken cancellationToken = default)
  {
    return await _db.Set<BookEntity>()
        .Select(b => b.Author)
        .Distinct()
        .OrderBy(a => a)
        .ToListAsync(cancellationToken);
  }
}
