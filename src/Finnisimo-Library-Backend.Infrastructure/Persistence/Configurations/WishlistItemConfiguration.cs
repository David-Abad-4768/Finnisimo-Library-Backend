using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class WishlistItemConfiguration
    : IEntityTypeConfiguration<WishlistItemEntity>
{
  public void Configure(EntityTypeBuilder<WishlistItemEntity> builder)
  {
    builder.ToTable("WishlistItems");

    builder.HasKey(wi => new { wi.UserId, wi.BookId });

    builder.HasOne(wi => wi.User)
        .WithMany(u => u.WishlistItems)
        .HasForeignKey(wi => wi.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(wi => wi.Book)
        .WithMany(b => b.WishlistItems)
        .HasForeignKey(wi => wi.BookId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
