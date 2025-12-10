using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrImportLineConfiguration : BaseImportLineEntityConfiguration<PrImportLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrImportLine> builder)
        {
            builder.ToTable("RII_PR_IMPORT_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.Property(x => x.LineId);

            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_PrImportLine_HeaderId");

            builder.HasIndex(x => x.LineId)
                .HasDatabaseName("IX_PrImportLine_LineId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_PrImportLine_StockCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PrImportLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Line)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
