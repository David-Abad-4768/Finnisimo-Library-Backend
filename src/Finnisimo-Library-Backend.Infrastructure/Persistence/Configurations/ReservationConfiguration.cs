using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class ReservationConfiguration
    : IEntityTypeConfiguration<ReservationEntity>
{
  public void Configure(EntityTypeBuilder<ReservationEntity> builder)
  {
    builder.ToTable("Reservations");

    builder.HasKey(r => r.Id);
    builder.Property(r => r.Id).ValueGeneratedNever();

    builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);

    builder.HasIndex(r => r.UserId);

    builder.HasIndex(r => new { r.UserId, r.BookId })
        .IsUnique()
        .HasFilter("\"Status\" = 'Active'");
  }
}
