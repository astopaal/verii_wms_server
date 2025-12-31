using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class UserDetailConfiguration : BaseEntityConfiguration<UserDetail>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserDetail> builder)
        {
            builder.ToTable("RII_USER_DETAIL");

            // Properties
            builder.Property(ud => ud.UserId)
                .IsRequired();

            builder.Property(ud => ud.ProfilePictureUrl)
                .HasMaxLength(500);

            builder.Property(ud => ud.Height)
                .HasColumnType("decimal(5,2)")
                .IsRequired(false);

            builder.Property(ud => ud.Weight)
                .HasColumnType("decimal(5,2)")
                .IsRequired(false);

            builder.Property(ud => ud.Description)
                .HasMaxLength(2000);

            builder.Property(ud => ud.Gender)
                .HasConversion<int>()
                .IsRequired(false);

            // Foreign key relationship - 1:1 relationship with User
            builder.HasOne(ud => ud.User)
                .WithOne(u => u.UserDetail)
                .HasForeignKey<UserDetail>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique index on UserId to ensure one detail per user
            builder.HasIndex(ud => ud.UserId)
                .IsUnique()
                .HasDatabaseName("IX_RII_USER_DETAIL_UserId");
        }
    }
}
