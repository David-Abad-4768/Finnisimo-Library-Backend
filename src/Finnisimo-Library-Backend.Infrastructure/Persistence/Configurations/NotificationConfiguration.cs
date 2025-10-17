using Finnisimo_Library_Backend.Domain.Entities.Notifications;
using Finnisimo_Library_Backend.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finnisimo_Library_Backend.Infrastructure.Persistence.Configurations;

internal sealed class NotificationConfiguration
    : IEntityTypeConfiguration<NotificationEntity>
{
  public void Configure(EntityTypeBuilder<NotificationEntity> builder)
  {
    builder.ToTable("Notifications");

    builder.HasKey(n => n.Id);
    builder.Property(n => n.Id).ValueGeneratedNever();

    builder.Property(n => n.Title).IsRequired().HasMaxLength(100);

    builder.Property(n => n.Message).IsRequired().HasMaxLength(500);

    builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);

    builder.Property(n => n.CreatedAt).IsRequired();

    builder.HasOne<UserEntity>()
        .WithMany()
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
