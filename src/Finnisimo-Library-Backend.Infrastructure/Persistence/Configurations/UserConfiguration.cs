using Finnisimo_Library_Backend.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
  public void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    builder.ToTable("Users");

    builder.HasKey(u => u.Id);
    builder.Property(u => u.Id).ValueGeneratedNever();

    builder.Property(u => u.Name).IsRequired().HasMaxLength(50);

    builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

    builder.Property(u => u.Email).IsRequired().HasMaxLength(100);

    builder.Property(u => u.Username).IsRequired().HasMaxLength(50);

    builder.Property(u => u.Password).IsRequired();

    builder.Property(u => u.PhotoUrl).IsRequired(false);

    builder.Property(u => u.CreatedAt).IsRequired();
    builder.Property(u => u.DateOfBirth).IsRequired();

    builder.HasIndex(u => u.Username).IsUnique();

    builder.HasIndex(u => u.Email).IsUnique();

    builder.HasMany(u => u.Loans)
        .WithOne(l => l.User)
        .HasForeignKey(l => l.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(u => u.Reservations)
        .WithOne(r => r.User)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
