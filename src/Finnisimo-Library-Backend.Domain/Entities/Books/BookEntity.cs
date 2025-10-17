using System.ComponentModel.DataAnnotations.Schema;
using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Books.Events;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;

namespace Finnisimo_Library_Backend.Domain.Entities.Books;

public sealed class BookEntity : BaseEntity
{
  private BookEntity(Guid id, string title, string author, string publisher,
                     DateOnly publicationDate, string description, string genre,
                     int numberOfPages, string language, string? coverImageUrl,
                     int stock, int initialStock, string location)
      : base(id)
  {
    Title = title;
    Author = author;
    Publisher = publisher;
    PublicationDate = publicationDate;
    Description = description;
    Genre = genre;
    NumberOfPages = numberOfPages;
    Language = language;
    CoverImageUrl = coverImageUrl;
    Stock = stock;
    InitialStock = initialStock;
    Location = location;
  }

  public string Title { get; private set; }
  public string Author { get; private set; }
  public string Publisher { get; private set; }
  public DateOnly PublicationDate { get; private set; }
  public string Description { get; private set; }
  public string Genre { get; private set; }
  public int NumberOfPages { get; private set; }
  public string Language { get; private set; }
  public string? CoverImageUrl { get; private set; }
  public int Stock { get; private set; }
  public int InitialStock { get; private set; }
  public string Location { get; private set; }
  public int TimesLoaned { get; private set; } = 0;

  [NotMapped]
  public string Availability
  {
    get
    {
      if (Stock == 0)
      {
        return "NoStock";
      }
      if (Stock <= (InitialStock / 2.0))
      {
        return "Medium";
      }
      return "High";
    }
  }

  public void IncrementLoanCount() { TimesLoaned++; }

  public void DecreaseStock(int quantity)
  {
    if (Stock >= quantity)
    {
      Stock -= quantity;
    }
    else
    {
    }
  }

  public void IncreaseStock(int quantity)
  {
    if (Stock + quantity <= InitialStock)
    {
      Stock += quantity;
    }
    else
    {
      Stock = InitialStock;
    }
  }

  public static BookEntity Create(string title, string author, string publisher,
                                  DateOnly publicationDate, string description,
                                  string genre, int numberOfPages,
                                  string language, string? coverImageUrl,
                                  int initialStock, string location)
  {
    var book = new BookEntity(
        Guid.NewGuid(), title, author, publisher, publicationDate, description,
        genre, numberOfPages, language, coverImageUrl, stock: initialStock,
        initialStock: initialStock, location);

    book.RaiseDomainEvent(new BookCreatedDomainEvent(book.Id));
    return book;
  }

  public ICollection<LoanEntity> Loans { get; private set; } = [];
  public ICollection<ReservationEntity> Reservations { get; private set; } = [];
  public ICollection<WishlistItemEntity> WishlistItems
  {
    get; private set;
  } = [];
}
