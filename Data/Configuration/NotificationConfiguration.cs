using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class NotificationConfiguration : BaseEntityConfiguration<Notification>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("RII_NOTIFICATION");

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.TitleKey).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.MessageKey).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.Channel)
                .IsRequired()
                .HasConversion<byte>(); // Convert enum to byte for database
            builder.Property(x => x.Severity)
                .IsRequired(false)
                .HasConversion<byte?>(); // Convert enum to byte for database
            builder.Property(x => x.IsRead).HasDefaultValue(false);

            builder.Property(x => x.ScheduledAt).IsRequired(false);
            builder.Property(x => x.DeliveredAt).IsRequired(false);
            builder.Property(x => x.ExpiresAt).IsRequired(false);

            builder.Property(x => x.ActionUrl).HasMaxLength(250);
            builder.Property(x => x.TerminalActionCode).HasMaxLength(50);
            builder.Property(x => x.RelatedEntityType).HasMaxLength(100);

            builder.HasIndex(x => x.Channel).HasDatabaseName("IX_Notification_Channel");
            builder.HasIndex(x => x.IsRead).HasDatabaseName("IX_Notification_IsRead");
            builder.HasIndex(x => x.DeliveredAt).HasDatabaseName("IX_Notification_DeliveredAt");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_Notification_IsDeleted");

            builder.HasOne(x => x.RecipientUser)
                .WithMany()
                .HasForeignKey(x => x.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
