using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.ReadingLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class ReadingLogEntryConfiguration
    : IEntityTypeConfiguration<ReadingLogEntryEntity>
{
  public void Configure(EntityTypeBuilder<ReadingLogEntryEntity> builder)
  {
    builder.ToTable("ReadingLogEntries");

    builder.HasKey(r => r.Id);
    builder.Property(r => r.Id).ValueGeneratedNever();

    builder.Property(r => r.StartPage).IsRequired();
    builder.Property(r => r.EndPage).IsRequired();
    builder.Property(r => r.PagesRead).IsRequired();
    builder.Property(r => r.DateLogged).IsRequired();

    builder.HasOne(r => r.Loan)
        .WithMany(l => l.ReadingLog)
        .HasForeignKey(r => r.LoanId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(r => r.UserId);
    builder.HasIndex(r => r.LoanId);
    builder.HasIndex(r => r.DateLogged);
  }
}
