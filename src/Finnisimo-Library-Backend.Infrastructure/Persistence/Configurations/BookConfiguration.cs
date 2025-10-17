using Finnisimo_Library_Backend.Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
  public void Configure(EntityTypeBuilder<BookEntity> builder)
  {
    builder.ToTable("Books");

    builder.HasKey(b => b.Id);
    builder.Property(b => b.Id).ValueGeneratedNever();

    builder.Property(b => b.Title).IsRequired().HasMaxLength(200);

    builder.Property(b => b.Author).IsRequired().HasMaxLength(100);

    builder.Property(b => b.Publisher).IsRequired().HasMaxLength(100);

    builder.Property(b => b.Description).IsRequired().HasMaxLength(1500);

    builder.Property(b => b.Genre).IsRequired().HasMaxLength(50);

    builder.Property(b => b.Language).IsRequired().HasMaxLength(50);

    builder.Property(b => b.Location).IsRequired().HasMaxLength(50);

    builder.Property(b => b.CoverImageUrl).IsRequired(false);

    builder.Property(b => b.Stock).HasDefaultValue(0);

    builder.Property(b => b.TimesLoaned).HasDefaultValue(0);

    builder.HasIndex(b => new { b.Title, b.Author }).IsUnique();

    builder.HasMany(b => b.Loans)
        .WithOne(l => l.Book)
        .HasForeignKey(l => l.BookId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(b => b.Reservations)
        .WithOne(r => r.Book)
        .HasForeignKey(r => r.BookId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
