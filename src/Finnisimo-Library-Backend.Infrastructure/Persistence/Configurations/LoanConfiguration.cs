using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class LoanConfiguration : IEntityTypeConfiguration<LoanEntity>
{
  public void Configure(EntityTypeBuilder<LoanEntity> builder)
  {
    builder.ToTable("Loans");

    builder.HasKey(l => l.Id);
    builder.Property(l => l.Id).ValueGeneratedNever();

    builder.Property(l => l.Status).HasConversion<string>().HasMaxLength(20);

    builder.HasIndex(l => l.UserId);
    builder.HasIndex(l => l.BookId);
  }
}
