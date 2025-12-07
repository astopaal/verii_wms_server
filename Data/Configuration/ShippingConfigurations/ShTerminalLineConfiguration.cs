using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class ShTerminalLineConfiguration : BaseEntityConfiguration<ShTerminalLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ShTerminalLine> builder)
        {
            builder.ToTable("RII_SH_TERMINAL_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            builder.Property(x => x.TerminalUserId).IsRequired();

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_ShTerminalLine_HeaderId");
            builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_ShTerminalLine_TerminalUserId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShTerminalLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.TerminalLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
