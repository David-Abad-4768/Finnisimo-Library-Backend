using Finnisimo_Library_Backend.Domain.Abstractions;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.Users.Events;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;

namespace Finnisimo_Library_Backend.Domain.Entities.Users;

public sealed class UserEntity : BaseEntity
{
  private UserEntity(Guid id, string name, string lastName, string email,
                     string username, string password, string photoUrl,
                     DateTime createdAt, DateTime dateOfBirth)
      : base(id)
  {
    Name = name;
    LastName = lastName;
    Email = email;
    Username = username;
    Password = password;
    PhotoUrl = photoUrl;
    CreatedAt = createdAt;
    DateOfBirth = dateOfBirth;
  }

  public string Name { get; private set; }
  public string LastName { get; private set; }
  public string Email { get; private set; }
  public string Username { get; private set; }
  public string Password { get; private set; }
  public string PhotoUrl { get; private set; }

  public DateTime CreatedAt { get; private set; }
  public DateTime DateOfBirth { get; private set; }

  public static UserEntity Create(string name, string lastName, string email,
                                  string username, string password,
                                  string photoUrl, DateTime dateOfBirth,
                                  DateTime createdAt)
  {
    var user = new UserEntity(Guid.NewGuid(), name, lastName, email, username,
                              password, photoUrl, createdAt, dateOfBirth);

    user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

    return user;
  }

  public void Update(string name, string lastName, string email,
                     string username)
  {
    Name = name;
    LastName = lastName;
    Email = email;
    Username = username;
  }

  public ICollection<LoanEntity> Loans { get; private set; } = [];
  public ICollection<ReservationEntity> Reservations { get; private set; } = [];
  public ICollection<WishlistItemEntity> WishlistItems
  {
    get; private set;
  } = [];
}
