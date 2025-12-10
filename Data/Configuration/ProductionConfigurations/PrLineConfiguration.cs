using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrLineConfiguration : BaseLineEntityConfiguration<PrLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrLine> builder)
        {
            builder.ToTable("RII_PR_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_PrLine_HeaderId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_PrLine_StockCode");

            builder.HasIndex(x => x.ErpOrderNo)
                .HasDatabaseName("IX_PrLine_ErpOrderNo");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PrLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SerialLines)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
