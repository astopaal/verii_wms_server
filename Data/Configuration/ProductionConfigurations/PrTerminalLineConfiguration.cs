using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrTerminalLineConfiguration : BaseEntityConfiguration<PrTerminalLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrTerminalLine> builder)
        {
            builder.ToTable("RII_PR_TERMINAL_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.Property(x => x.TerminalUserId)
                .IsRequired();

            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_PrTerminalLine_HeaderId");

            builder.HasIndex(x => x.TerminalUserId)
                .HasDatabaseName("IX_PrTerminalLine_TerminalUserId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PrTerminalLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany()
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.TerminalUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
