using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;
namespace WMS_WEBAPI.Data.Configuration
{
    public class PasswordResetRequestConfiguration : BaseEntityConfiguration<PasswordResetRequest>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PasswordResetRequest> builder)
        {
            builder.ToTable("RII_PASSWORD_RESET_REQUEST");
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(128);
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.Property(x => x.UsedAt).IsRequired(false);
            builder.Property(x => x.RequestIp).HasMaxLength(100);
            builder.Property(x => x.UserAgent).HasMaxLength(500);
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.UserId, x.TokenHash });
            builder.HasIndex(x => x.ExpiresAt);
        }
    }
}
